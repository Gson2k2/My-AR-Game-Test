using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LimWorks.Rendering.URP.ScreenSpaceReflections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ShadowQuality = UnityEngine.ShadowQuality;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;


public class SettingManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject settingBtn;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Light light;

    [SerializeField] private TMP_Dropdown dynamicResDropDown;
    [SerializeField] private TMP_Dropdown shadowDropDown;
    [SerializeField] private TMP_Dropdown antiAliasingDropDown;
    [SerializeField] private TMP_Dropdown ssrDropDown;

    private UniversalAdditionalCameraData _cameraData;
    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);
        _cameraData = _camera.GetComponent<UniversalAdditionalCameraData>();
        
        if (PlayerPrefs.GetInt("Config") == 1)
        {
            string jsonData = PlayerPrefs.GetString("MySettings");
            Save loadedData = JsonUtility.FromJson<Save>(jsonData);
        
            _camera.allowDynamicResolution = loadedData.DynamicResolution;
            light.shadows = loadedData.ShadowResolution;
            _cameraData.antialiasing = loadedData.AntiAliasing;

            OnDropDownPreset(dynamicResDropDown,loadedData.DynamicResolution);
            OnDropDownShadowPreset(shadowDropDown,loadedData.ShadowResolution);
            OnDropDownAntialiasingPreset(antiAliasingDropDown, loadedData.AntiAliasing);
        }
        else
        {
            PlayerPrefs.SetInt("Config",1);
            
            _camera.allowDynamicResolution = true;
            light.shadows = LightShadows.Hard;
            _cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            Save saveData = new Save
            {
                ShadowResolution = LightShadows.Hard,
                AntiAliasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing,
                DynamicResolution = true
            };
            
            string saveDataJson = JsonUtility.ToJson(saveData);
            
            PlayerPrefs.SetString("MySettings", saveDataJson);
            PlayerPrefs.Save();
        }
    }

    [Button("Clear Pref")]
    public void Test()
    {
        PlayerPrefs.DeleteAll();
    }
    
    public void OnDropDownShadowPreset(TMP_Dropdown dropdown,LightShadows value)
    {
        List<TMP_Dropdown.OptionData> options = dropdown.options;

        string text = "null";
        switch (value)
        {
            case LightShadows.None:
                text = "Off";
                break;
            case LightShadows.Hard:
                text = "On";
                break;
        }

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text == text)
            {
                dropdown.value = i;
                dropdown.RefreshShownValue();
                break;
            }
        }
    }
    
    public void OnDropDownAntialiasingPreset(TMP_Dropdown dropdown,AntialiasingMode value)
    {
        List<TMP_Dropdown.OptionData> options = dropdown.options;

        string text = "null";
        switch (value)
        {
            case AntialiasingMode.FastApproximateAntialiasing:
                text = "FXAA";
                break;
            case AntialiasingMode.SubpixelMorphologicalAntiAliasing:
                text = "SMAA";
                break;
            case AntialiasingMode.TemporalAntiAliasing:
                text = "TAA";
                break;
            case AntialiasingMode.None:
                text = "Off";
                break;
        }

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text == text)
            {
                dropdown.value = i;
                dropdown.RefreshShownValue();
                break;
            }
        }
    }

    public void OnDropDownPreset(TMP_Dropdown dropdown,bool value)
    {
        List<TMP_Dropdown.OptionData> options = dropdown.options;

        string text;
        text = value ? "On" : "Off";
        
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text == text)
            {
                dropdown.value = i;
                dropdown.RefreshShownValue();
                break;
            }
        }
    }


    public void OnRestart()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void OnSettingDisable()
    {
        settingBtn.SetActive(false);
    }

    public void OnSettingClick()
    {
        settingPanel.SetActive(true);
        ItemFaceTracking.Instance.trackingIsDisable = true;
        RoadController.Instance.isDisable = true;
    }

    public void OnBackSetting()
    {
        settingPanel.SetActive(false);
        ItemFaceTracking.Instance.trackingIsDisable = false;
        RoadController.Instance.isDisable = false;
    }
    
    public void OnSSRSetting(TMP_Dropdown dropdown)
    {
        // var value = dropdown.value;
        // var selectedName = dropdown.options[value].text;
        //
        // bool boolValue;
        // LimSSR.Enabled = selectedName switch
        // {
        //     "On" => true,
        //     "Off" => false,
        //     _ => LimSSR.Enabled
        // };
        //
        // string jsonData = PlayerPrefs.GetString("MySettings");
        // Save loadedData = JsonUtility.FromJson<Save>(jsonData);
        // loadedData.SSR = LimSSR.Enabled;
        // string saveDataJson = JsonUtility.ToJson(loadedData);
        //
        // PlayerPrefs.SetString("MySettings", saveDataJson);
        // PlayerPrefs.Save();
    }
    

    public void OnShadowSetting(TMP_Dropdown dropdown)
    {
        var value = dropdown.value;
        var selectedName = dropdown.options[value].text;

        light.shadows = selectedName switch
        {
            "On" => LightShadows.Hard,
            "Off" => LightShadows.None,
            _ => light.shadows
        };

        string jsonData = PlayerPrefs.GetString("MySettings");
        Save loadedData = JsonUtility.FromJson<Save>(jsonData);
        loadedData.ShadowResolution = light.shadows;
        string saveDataJson = JsonUtility.ToJson(loadedData);
        
        PlayerPrefs.SetString("MySettings", saveDataJson);
        PlayerPrefs.Save();
    }
    public void OnDynamicResSetting(TMP_Dropdown dropdown)
    {
        var value = dropdown.value;
        var selectedName = dropdown.options[value].text;
        _camera.allowDynamicResolution = selectedName switch
        {
            "On" => true,
            "Off" => false,
            _ => _camera.allowDynamicResolution
        };
        
        string jsonData = PlayerPrefs.GetString("MySettings");
        Save loadedData = JsonUtility.FromJson<Save>(jsonData);
        _camera.allowDynamicResolution = _camera.allowDynamicResolution;
        string saveDataJson = JsonUtility.ToJson(loadedData);
        
        PlayerPrefs.SetString("MySettings", saveDataJson);
        PlayerPrefs.Save();
    }
    public void OnAntiAliasingSetting(TMP_Dropdown dropdown)
    {
        var value = dropdown.value;
        var selectedName = dropdown.options[value].text;

        _cameraData.antialiasing = selectedName switch
        {
            "FXAA" => AntialiasingMode.FastApproximateAntialiasing,
            "SMAA" => AntialiasingMode.SubpixelMorphologicalAntiAliasing,
            "TAA" => AntialiasingMode.TemporalAntiAliasing,
            "Off" => AntialiasingMode.None,
            _ => _cameraData.antialiasing
        };
        
        string jsonData = PlayerPrefs.GetString("MySettings");
        Save loadedData = JsonUtility.FromJson<Save>(jsonData);
        _cameraData.antialiasing = _cameraData.antialiasing;
        string saveDataJson = JsonUtility.ToJson(loadedData);
        
        PlayerPrefs.SetString("MySettings", saveDataJson);
        PlayerPrefs.Save();
    }
}
[Serializable]
public class Save
{
    public bool DynamicResolution;
    public AntialiasingMode AntiAliasing;
    public LightShadows ShadowResolution;
}
