using System.Collections.Generic;
using Hertzole.ScriptableValues;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class ListTestUi : MonoBehaviour
{
	[SerializeField]
	private ScriptableGameObjectList scriptableList = default;

	public List<GameObject> list = new List<GameObject>();

	private int counter = 0;
	private ListView basicListView;
	private ListView scriptableListView;

	private UIDocument ui;

	private void Awake()
	{
		counter = 0;
		ui = GetComponent<UIDocument>();
	}

	private void Start()
	{
		basicListView = ui.rootVisualElement.Q<ListView>("basic-list");
		scriptableListView = ui.rootVisualElement.Q<ListView>("scriptable-list");

		basicListView.dataSource = this;
		basicListView.onAdd += OnAdd;

		scriptableListView.dataSource = this;
		scriptableListView.onAdd += OnAdd;

		basicListView.SetBinding("itemsSource", new DataBinding { dataSourcePath = new PropertyPath(nameof(list)) });
		scriptableListView.SetBinding("itemsSource", new DataBinding { dataSourcePath = new PropertyPath(nameof(scriptableList)) });
	}

	private void OnAdd(BaseListView obj)
	{
		GameObject go = new GameObject("Obj " + counter);

		list.Add(go);
		scriptableList.Add(go);
		counter++;
		
		scriptableListView.RefreshItems();
	}
}