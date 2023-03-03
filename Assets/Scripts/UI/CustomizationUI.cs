using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CustomizationUI : MonoBehaviour
{
    public CustomBallUIElement template;

    private CustomBalls _balls;
    private IStorage _storage;
    private SignalBus _signalBus;

    private Transform _parent;

    private Dictionary<string, CustomBallUIElement> _uiElements = new Dictionary<string, CustomBallUIElement>();

    [Inject]
    protected void Construct(CustomBalls customBalls, IStorage storage, SignalBus signalBus)
    {
        _balls = customBalls;
        _storage = storage;
        _signalBus = signalBus;
    }

    private void Start()
    {
        _parent = template.transform.parent;
        template.gameObject.SetActive(false);
        CreateCustomBalls();
        Refresh();
    }

    private void CreateCustomBalls()
    {
        _uiElements.Add(_balls.defaultBall.id, CreateCustomBall(_balls.defaultBall));
        _balls.balls.ForEach(b => _uiElements.Add(b.id, CreateCustomBall(b)));
    }

    private CustomBallUIElement CreateCustomBall(CustomBalls.Data ball)
    {
        var ui = Instantiate(template, _parent);

        ui.image.sprite = ball.icon;
        ui.text.text = ball.name;

        var id = ball.id;
        ui.toggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
                OnStyleSelected(id);
        });

        ui.gameObject.SetActive(true);

        return ui;
    }

    private void OnStyleSelected(string id)
    {
        _storage.GetState().selectedBallId = id;
        _storage.Save();
        Refresh();
    }

    private void Refresh()
    {
        var selectedId = _storage.GetState().selectedBallId;
        foreach (var element in _uiElements)
        {
            element.Value.toggle.isOn = element.Key == selectedId;
        }
    }
}
