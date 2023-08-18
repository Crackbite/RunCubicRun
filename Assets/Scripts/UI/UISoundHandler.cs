using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UISoundHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private SoundSystem _soundSystem;
    [SerializeField] private StoreScreen _storeScreen;

    private void OnEnable()
    {
        SetupButtonEventHandlers(EventTriggerType.PointerDown);
        SetupButtonEventHandlers(EventTriggerType.PointerUp);
        _storeScreen.SkinViewsFilled += OnSkinViewsFilled;
    }

    private void OnDisable()
    {
        RemoveButtonEventHandlers();
        _storeScreen.SkinViewsFilled -= OnSkinViewsFilled;

        foreach (SkinView view in _storeScreen.SkinViews)
        {
            view.ChooseButtonClick -= OnSkinChooseButtonClick;
        }
    }

    private void SetupButtonEventHandlers(EventTriggerType eventTriggerType)
    {
        foreach (Button button in _buttons)
        {
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                Debug.LogWarning("EventTrigger component is missing on button: " + button.name);
                continue;
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback.AddListener((data) => { OnButtonEvent(eventTriggerType); });
            trigger.triggers.Add(entry);
        }
    }

    private void RemoveButtonEventHandlers()
    {
        foreach (Button button in _buttons)
        {
            if (button == null)
            {
                continue;
            }

            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

            if (trigger != null)
            {
                trigger.triggers.Clear();
            }
        }
    }

    private void OnButtonEvent(EventTriggerType eventTriggerType)
    {
        if (eventTriggerType == EventTriggerType.PointerDown)
        {
            _soundSystem.Play(SoundEvent.PointerDown);
        }
        else
        {
            _soundSystem.Play(SoundEvent.PointerUp);
        }
    }

    private void OnSkinViewsFilled()
    {
        foreach (SkinView view in _storeScreen.SkinViews)
        {
            view.ChooseButtonClick += OnSkinChooseButtonClick;
        }
    }

    private void OnSkinChooseButtonClick(Skin skin, SkinView skinView)
    {
        _soundSystem.Play(SoundEvent.SkinSelect);
    }
}
