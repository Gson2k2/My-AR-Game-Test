using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayControlller : MonoBehaviour
{
    public static GamePlayControlller Instance;
    [SerializeField] private TextMeshProUGUI gZeniText;
    [SerializeField] private TextMeshProUGUI sZeniText;
    [SerializeField] private Slider slider;

    [SerializeField] private GameData _gameData;
    
    private void OnEnable()
    {
        Instance = this;
        _gameData = new GameData();
        _gameData.SZeniPlus(0);
        _gameData.GZeniPlus(0);
        
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void OnPlay(Canvas playCanvas)
    {
        ItemFaceTracking.Instance.trackingIsDisable = false;
        RoadController.Instance.isDisable = false;
        playCanvas.gameObject.SetActive(false);
    }

    [Button("Test")]
    public void TestUpdateZeni()
    {
        OnZeniUpdate(ItemType.SZeni, 500);
    }

    private CancellationTokenSource _cancellationTokenSource;
    private Sequence sZeniSequence;


    private int tempValue;
    public async void OnZeniUpdate(ItemType itemType,int value)
    {
        switch (itemType)
        {
            case ItemType.GZeni:
                _gameData.GZeniPlus(value);
                gZeniText.text = _gameData.GZeniGet().ToString();
                break;
            case ItemType.SZeni:

                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();

                
                int tempSZeni = _gameData.SZeniGet();

                _gameData.SZeniPlus(value);
                DOTween.To(() => tempSZeni, x =>tempSZeni = x, 
                        _gameData.SZeniGet(), 1f)
                    .OnUpdate(() =>
                    {
                        sZeniText.text = tempSZeni.ToString();
                    })
                    .WithCancellation(_cancellationTokenSource.Token);
                
                await DOTween.To(() => slider.value, x => slider.value = x, 
                        _gameData.SZeniGet(), 1f).SetEase(Ease.Linear)
                    .WithCancellation(_cancellationTokenSource.Token);
                
                _cancellationTokenSource.Cancel();
                break;
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }
}

[Serializable]
public struct GameData
{
    public int gZeni;
    public int sZeni;
    
    
    public int GZeniPlus(int value)
    {
        gZeni += value;
        return sZeni;
    }
    public int SZeniPlus(int value)
    {
        sZeni += value;
        return sZeni;
    }

    public int GZeniGet()
    {
        return gZeni;
    }
    public int SZeniGet()
    {
        return sZeni;
    }
}
