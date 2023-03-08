using System;
using Lean.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace Lean.Touch
{
	/// <summary>
	///     This component fires events on the first frame where a finger has been touching the screen for more than
	///     <b>TapThreshold</b> seconds, and is therefore no longer eligible for tap or swipe events.
	/// </summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanFingerOld")]
    [AddComponentMenu(LeanTouch.ComponentPathPrefix + "Finger Old")]
    public class LeanFingerOld : MonoBehaviour
    {
        /// <summary>Ignore fingers with StartedOverGui?</summary>
        public bool IgnoreStartedOverGui = true;

        /// <summary>Ignore fingers with OverGui?</summary>
        public bool IgnoreIsOverGui;

        /// <summary>Do nothing if this LeanSelectable isn't selected?</summary>
        public LeanSelectable RequiredSelectable;

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

        /// <summary>This event will be called if the above conditions are met when your finger becomes old.</summary>
        public LeanFingerEvent OnFinger
        {
            get
            {
                if (onFinger == null) onFinger = new LeanFingerEvent();
                return onFinger;
            }
        }

        /// <summary>
        ///     This event will be called if the above conditions are met when your finger becomes old.
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
        ///     This event will be called if the above conditions are met when your finger becomes old.
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
            LeanTouch.OnFingerOld += HandleFingerOld;
        }

        protected virtual void OnDisable()
        {
            LeanTouch.OnFingerOld -= HandleFingerOld;
        }

        private void HandleFingerOld(LeanFinger finger)
        {
            // Ignore?
            if (IgnoreStartedOverGui && finger.StartedOverGui) return;

            if (IgnoreIsOverGui && finger.IsOverGui) return;

            if (RequiredSelectable != null && RequiredSelectable.IsSelected == false) return;

            if (onFinger != null) onFinger.Invoke(finger);

            if (onWorld != null)
            {
                var position = ScreenDepth.Convert(finger.ScreenPosition, gameObject);

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
    [CustomEditor(typeof(LeanFingerOld))]
    public class LeanFingerOld_Inspector : LeanInspector<LeanFingerOld>
    {
        private bool showUnusedEvents;

        protected override void DrawInspector()
        {
            Draw("IgnoreStartedOverGui", "Ignore fingers with StartedOverGui?");
            Draw("IgnoreIsOverGui", "Ignore fingers with OverGui?");
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