using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionPanelDriver : MonoBehaviour
{
    [SerializeField] List<Image> _costImages = null;
    [SerializeField] Image _backgroundImage = null;
    [SerializeField] Sprite _affordableBGSprite = null;
    [SerializeField] Sprite _tooMuchCostBGSprite = null;
    public void SetCost(int cost, bool canAfford)
    {
        foreach (var image in _costImages)
        {
            image.enabled = false;
        }
        for (int i = 0; i < cost; i++)
        {
            _costImages[i].enabled = true;
        }
        
        if (canAfford)
        {
            _backgroundImage.sprite = _affordableBGSprite;
        }
        else
        {
            _backgroundImage.sprite = _tooMuchCostBGSprite;
        }
    }
}
