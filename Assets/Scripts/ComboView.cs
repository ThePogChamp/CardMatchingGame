using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboView : MonoBehaviour
{
    [SerializeField] private BoardPresenter presenter;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private GameObject comboContainer;

    private void Awake()
    {
        comboContainer.SetActive(false);
    }

    private void OnEnable()
    {
        presenter.OnComboChanged += UpdateCombo;
        presenter.OnGameStarted += HideCombo;
    }

    private void OnDisable()
    {
        presenter.OnComboChanged -= UpdateCombo;
        presenter.OnGameStarted -= HideCombo;
    }

    private void UpdateCombo(int combo)
    {
        if (combo <= 1)
        {
            comboContainer.SetActive(false);
            return;
        }

        comboContainer.SetActive(true);
        comboText.text = $"COMBO x{combo}";
    }

    private void HideCombo()
    {
        comboContainer.SetActive(false);
    }
}
