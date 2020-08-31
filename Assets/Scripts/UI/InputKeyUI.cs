using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputKeyUI : MonoBehaviour
{
    public const string KeyboardSchemeName = "Keyboard";
    public const string PS4SchemeName = "PS4";
    public const string XBoxSchemeName = "XBox";

    public Image icon;
    public TextMeshProUGUI iconText;
    public TextMeshProUGUI actionText;
    public Color disabledColor;

    [Header("Input Sources")]
    public PlayerInput playerInput;
    public InputActionAsset inputActionAsset;
    public string inputActionMapName;

    public bool useInputActionAsset => playerInput == null;

    [Header("XBox Mapping")]
    public Sprite xBoxSprite;
    public string xBoxText;

    [Header("PS4 Mapping")]
    public Sprite ps4Sprite;
    public string ps4Text;

    [Header("Keyboard Mapping")]
    public Sprite keyboardSprite;
    public string keyboardText;

    private string lastSeenControlScheme = "";
    private InputActionMap inputActionMap = null;


    // Start is called before the first frame update
    void Start()
    {
        if (inputActionAsset)
        {
            foreach (var actionMap in inputActionAsset.actionMaps)
            {
                if (actionMap.name.Equals(inputActionMapName))
                {
                    inputActionMap = actionMap;
                    break;
                }
            }
        }

        /*if (inputActionMap != null && useInputActionAsset)
        {
            inputActionMap.actionTriggered += context =>
            {
                var scheme = InputControlScheme.FindControlSchemeForDevice(context.control.device,
                    inputActionMap.controlSchemes);
                if (scheme.HasValue)
                {
                    OnControlSchemeChanged(scheme.Value.name);
                }
            };

            inputActionMap.Enable();
        }*/

        OnControlSchemeChanged(KeyboardSchemeName);

        OnPlayerInputChanged();
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    private void OnPlayerInputChanged()
    {
        if (playerInput)
        {
            playerInput.onControlsChanged += OnControlsChanged;
        }

        Refresh();
    }

    public void SetPlayerInput(PlayerInput input)
    {
        if (playerInput)
        {
            playerInput.onControlsChanged -= OnControlsChanged;
        }

        playerInput = input;
        OnPlayerInputChanged();
    }

    public void OnControlsChanged(PlayerInput input)
    {
        Refresh();
    }

    public void Enable(bool enable)
    {
        icon.color = enable ? Color.white : disabledColor;
    }

    public void SetActionText(string text)
    {
        actionText.text = text;
    }

    public void Refresh()
    {
        if (useInputActionAsset)
        {
            if (inputActionMap != null && GameManager.Instance.lastDetectedDevice != null)
            {
                var scheme = InputControlScheme.FindControlSchemeForDevice(GameManager.Instance.lastDetectedDevice,
                    inputActionMap.controlSchemes);
                if (scheme.HasValue)
                {
                    OnControlSchemeChanged(scheme.Value.name);
                }
            }
        }
        else if (playerInput)
        {
            OnControlSchemeChanged(playerInput.currentControlScheme);
        }
    }

    void OnControlSchemeChanged(string newControlScheme)
    {
        if (newControlScheme == null || newControlScheme.Equals(lastSeenControlScheme))
        {
            return;
        }

        Sprite sprite;
        string text;

        switch (newControlScheme)
        {
            case KeyboardSchemeName:
                sprite = keyboardSprite;
                text = keyboardText;
                break;
            case PS4SchemeName:
                sprite = ps4Sprite;
                text = ps4Text;
                break;
            default:
                sprite = xBoxSprite;
                text = xBoxText;
                break;
        }

        if (sprite)
        {
            icon.sprite = sprite;
            iconText.text = text;
        }

        lastSeenControlScheme = newControlScheme;
    }
}