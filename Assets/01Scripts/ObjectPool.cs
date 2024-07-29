using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> objects = new Queue<T>();
    private Transform poolTransform; // Pool�� �θ� Ʈ������

    public ObjectPool(T prefab, int initialSize, Transform poolTransform)
    {
        this.prefab = prefab;
        this.poolTransform = poolTransform;

        for (int i = 0; i < initialSize; i++)
        {
            T newObject = GameObject.Instantiate(prefab);
            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(poolTransform); // Pool�� �θ� Ʈ������ ����
            InitializeObject(newObject); // �ʱ�ȭ �޼��� ȣ��
            objects.Enqueue(newObject);
        }
    }

    public T Get()
    {
        T obj;
        if (objects.Count > 0)
        {
            obj = objects.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(prefab);
            obj.transform.SetParent(poolTransform); // Pool�� �θ� Ʈ������ ����
        }
        InitializeObject(obj); // �ʱ�ȭ �޼��� ȣ��
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }

    private void InitializeObject(T obj)
    {
        
        if (obj is TextMeshProUGUI textMeshPro)
        {
            textMeshPro.fontSize = 20;
            textMeshPro.rectTransform.anchoredPosition = Vector2.zero; // ���ϴ� �⺻ ��ġ�� ����
            textMeshPro.rectTransform.localScale = Vector3.one; // �⺻ �����Ϸ� ����
        }
    }
}