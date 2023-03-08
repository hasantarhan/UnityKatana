using System;
using Lean.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
    /// <summary>This component calls the OnUp event when a finger stops touching the screen.</summary>
    [HelpURL(LeanTouch.HelpUrlPrefix + "LeanFingerUp")]
    [AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Up")]
    public class LeanFingerUp : MonoBehaviour
    {
        /// <summary>Ignore fingers with StartedOverGui?</summary>
        public bool IgnoreStartedOverGui = true;

        /// <summary>Ignore fingers with IsOverGui?</summary>
        public bool IgnoreIsOverGui;

        /// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
        public LeanSelectable RequiredSelectable;

        [FSA("onUp")]
        [FSA("OnUp")]
        [SerializeField]
        private LeanFingerEvent onFinger;

        /// <summary>
        ///     The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more
        ///     information.
        /// </summary>
        public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

        [SerializeField]
        [FSA("onPosition")]
        private Vector3Event onWorld;

        [SerializeField]
        private Vector2Event onScreen;

        /// <summary>This event will be called if the above conditions are met when your finger stops touching the screen.</summary>
        public LeanFingerEvent OnFinger
        {
            get
            {
                if (onFinger == null) onFinger = new LeanFingerEvent();
                return onFinger;
            }
        }

        /// <summary>
        ///     This event will be called if the above conditions are met when your finger stops touching the screen.
        ///     Vector3 = Finger position in world space.
        /// </summary>
        public Vector3Event OnWorld
        {
            get
            {
                if (onWorld == null) onWorld = new Vector3Event();
                return onWorld;
            }
        }

        /// <summary>
        ///     This event will be called if the above conditions are met when your finger stops touching the screen.
        ///     Vector2 = Finger position in screen space.
        /// </summary>
        public Vector2Event OnScreen
        {
            get
            {
                if (onScreen == null) onScreen = new Vector2Event();
                return onScreen;
            }
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            RequiredSelectable = GetComponentInParent<LeanSelectable>();
        }
#endif

        protected virtual void Start()
        {
            if (RequiredSelectable == null) RequiredSelectable = GetComponentInParent<LeanSelectable>();
        }

        protected virtual void OnEnable()
        {
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        protected virtual void OnDisable()
        {
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        private void HandleFingerUp(LeanFinger finger)
        {
            if (IgnoreStartedOverGui && finger.StartedOverGui) return;

            if (IgnoreIsOverGui && finger.IsOverGui) return;

            if (RequiredSelectable != null && RequiredSelectable.IsSelected == false) return;

            if (onFinger != null) onFinger.Invoke(finger);

            if (onWorld != null)
            {
                var position = ScreenDepth.Convert(finger.StartScreenPosition, gameObject);

                onWorld.Invoke(position);
            }

            if (onScreen != null) onScreen.Invoke(finger.ScreenPosition);
        }

        [Serializable]
        public class LeanFingerEvent : UnityEvent<LeanFinger>
        {
        }

        [Serializable]
        public class Vector3Event : UnityEvent<Vector3>
        {
        }

        [Serializable]
        public class Vector2Event : UnityEvent<Vector2>
        {
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Touch.Inspector
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LeanFingerUp))]
    public class LeanFingerUp_Inspector : LeanInspector<LeanFingerUp>
    {
        private bool showUnusedEvents;

        protected override void DrawInspector()
        {
            Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
            Draw("IgnoreIsOverGui", "Ignore fingers with IsOverGui?");
            Draw("RequiredSelectable", "Do nothing if this LeanSelectable isn't selected?");

            EditorGUILayout.Separator();

            var usedA = Any(t => t.OnFinger.GetPersistentEventCount() > 0);
            var usedB = Any(t => t.OnWorld.GetPersistentEventCount() > 0);
            var usedC = Any(t => t.OnScreen.GetPersistentEventCount() > 0);

            EditorGUI.BeginDisabledGroup(usedA && usedB && usedC);
            showUnusedEvents = EditorGUILayout.Foldout(showUnusedEvents, "Show Unused Events");
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            if (usedA || showUnusedEvents) Draw("onFinger");

            if (usedB || showUnusedEvents)
            {
                Draw("ScreenDepth");
                Draw("onWorld");
            }

            if (usedC || showUnusedEvents) Draw("onScreen");
        }
    }
}
#endif