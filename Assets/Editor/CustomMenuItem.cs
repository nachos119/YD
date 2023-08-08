using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomMenuItem
{
    //�ش� ��ũ��Ʈ�� �߰��� ������Ʈ�� transform
    static private Transform ScrollViewRootTf;

    //������ ������Ʈ�� �θ� �Ǻ��ϱ� ���� int ����
    static private int firstChild = 1;
    static private int secondChild = 2;
    static private int ThirdChild = 3;

    [MenuItem("GameObject/UI/CustomScrollView", false)]
    static void CreateCustomScrollView(MenuCommand menuCommand)
    {
        // 1. Custom GameObject �̸����� �� Object�� �����.
        GameObject go = new GameObject("CustomScrollView");
        ScrollViewRootTf = go.transform;
        // 2. Hierachy �����쿡�� � ������Ʈ�� �����Ͽ� �����ÿ��� �� ������Ʈ�� ���� �������� �����ȴ�.
        // �׹��� ��쿡�� �ƹ��ϵ� �Ͼ�� �ʴ´�.
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        //������Ʈ ���� �� ������ ������Ʈ RectTransform������Ʈ�� ����.
        CreateObject("Viewport", firstChild, 1);
        CreateObject("ScrollBarHorizontal", firstChild, 1);
        CreateObject("ScrollBarVertical", firstChild, 1);
        CreateObject("Content", secondChild, 1);
        CreateObject("SlidingAreaHorizontal", secondChild, 2);
        CreateObject("HandleHorizontal", ThirdChild, 2);
        CreateObject("SlidingAreaVertical", secondChild, 3);
        CreateObject("HandleVertical", ThirdChild, 3);

        FindChildByName("ScrollBarHorizontal", ScrollViewRootTf).AddComponent<HorizontalScrollBar>();
        FindChildByName("ScrollBarVertical", ScrollViewRootTf).AddComponent<VerticalScrollBar>();
        // 3. ������ ������Ʈ�� Undo �ý��ۿ� ����Ѵ�.
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);        
        go.AddComponent<CustomScrollRect>();        
        var rect = go.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200f, 200f);        
    }

    static private void CreateObject(string name, int ParentIndex, int ParentIndex2)
    {
        GameObject _newObejct = new GameObject(name);
        _newObejct.AddComponent<RectTransform>();
        var tf = _newObejct.transform;
        // 1 : firstChild
        // 2 : secondChild
        // 3 : ThirdChild
        switch (ParentIndex)
        {
            case 1:
                tf.SetParent(ScrollViewRootTf);
                break;
            case 2:
                if (ParentIndex2 == 1)
                    tf.SetParent(ScrollViewRootTf.GetChild(0).transform);
                else if (ParentIndex2 == 2)
                    tf.SetParent(ScrollViewRootTf.GetChild(1).transform);
                else
                    tf.SetParent(ScrollViewRootTf.GetChild(2).transform);
                break;
            case 3:
                if (ParentIndex2 == 1)
                    tf.SetParent(ScrollViewRootTf.GetChild(0).transform.GetChild(0).transform);
                else if (ParentIndex2 == 2)
                    tf.SetParent(ScrollViewRootTf.GetChild(1).transform.GetChild(0).transform);
                else
                    tf.SetParent(ScrollViewRootTf.GetChild(2).transform.GetChild(0).transform);
                break;            
        }
        _newObejct.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        _newObejct.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    static private GameObject FindChildByName(string ThisName, Transform ThisGObj)
    {
        GameObject ReturnObj;
        if (ThisGObj.name == ThisName)
            return ThisGObj.gameObject;
        foreach (Transform child in ThisGObj)
        {
            ReturnObj = FindChildByName(ThisName, child);
            if (ReturnObj != null)
                return ReturnObj;
        }
        return null;
    }
}
