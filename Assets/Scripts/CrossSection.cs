using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrossSection : MonoBehaviour
{
    [SerializeField] private float m_MaxX = 5.0f;
    [SerializeField] private float m_MaxY = 5.0f;
    [SerializeField] private float m_MaxZ = 5.0f;
    [SerializeField] private float m_MinX = -5.0f;
    [SerializeField] private float m_MinY = -5.0f;
    [SerializeField] private float m_MinZ = -5.0f;

    [SerializeField]
    private GameObject m_BoxSlicer = null;

    [SerializeField]
    private Transform m_Box = null;

    private List<Transform> m_listBoxSlicer = new List<Transform>();
    private List<Vector4> m_listBoxSlicerPositions = new List<Vector4>();
    private List<Vector4> m_listBoxSlicerNormals = new List<Vector4>();

    [SerializeField, Space(15)]
    private GameObject m_PlaneSlicerPrefab = null;

    private int PLANE_SLICER_NUM = 6;

    [SerializeField]
    private List<Transform> m_listPlaneSlicer = new List<Transform>();

    [SerializeField]
    private List<Vector4> m_listPlaneSlicerPositions = new List<Vector4>();
    
    [SerializeField]
    private List<Vector4> m_listPlaneSlicerNormals = new List<Vector4>();

    [SerializeField]
    private List<bool> m_listPlaneSliceFlag = new List<bool>();

    private List<float> m_listPlaneSliceShaderFlag = new List<float>();

    private void OnValidate()
    {
        m_BoxSlicer.transform.GetChild(0).position = new Vector3(m_MaxX, 0.0f, 0.0f);
        m_BoxSlicer.transform.GetChild(1).position = new Vector3(0.0f, m_MaxY, 0.0f);
        m_BoxSlicer.transform.GetChild(2).position = new Vector3(0.0f, 0.0f, m_MaxZ);
        m_BoxSlicer.transform.GetChild(3).position = new Vector3(m_MinX, 0.0f, 0.0f);
        m_BoxSlicer.transform.GetChild(4).position = new Vector3(0.0f, m_MinY, 0.0f);
        m_BoxSlicer.transform.GetChild(5).position = new Vector3(0.0f, 0.0f, m_MinZ);

        Shader.SetGlobalVector("g_MaxXPos", m_BoxSlicer.transform.GetChild(0).position);
        Shader.SetGlobalVector("g_MaxYPos", m_BoxSlicer.transform.GetChild(1).position);
        Shader.SetGlobalVector("g_MaxZPos", m_BoxSlicer.transform.GetChild(2).position);
        Shader.SetGlobalVector("g_MinXPos", m_BoxSlicer.transform.GetChild(3).position);
        Shader.SetGlobalVector("g_MinYPos", m_BoxSlicer.transform.GetChild(4).position);
        Shader.SetGlobalVector("g_MinZPos", m_BoxSlicer.transform.GetChild(5).position);

        Shader.SetGlobalVector("g_MaxXNor", m_BoxSlicer.transform.GetChild(0).up);
        Shader.SetGlobalVector("g_MaxYNor", m_BoxSlicer.transform.GetChild(1).up);
        Shader.SetGlobalVector("g_MaxZNor", m_BoxSlicer.transform.GetChild(2).up);
        Shader.SetGlobalVector("g_MinXNor", m_BoxSlicer.transform.GetChild(3).up);
        Shader.SetGlobalVector("g_MinYNor", m_BoxSlicer.transform.GetChild(4).up);
        Shader.SetGlobalVector("g_MinZNor", m_BoxSlicer.transform.GetChild(5).up);
    }

    private void Awake()
    {
        for (int i = 0; i < PLANE_SLICER_NUM; i++)
        {
            GameObject slicer = Instantiate(m_PlaneSlicerPrefab, transform);
            m_listPlaneSlicer.Add(slicer.transform);
            m_listPlaneSlicerPositions.Add(Vector4.zero);
            m_listPlaneSlicerNormals.Add(Vector4.zero);
            m_listPlaneSliceFlag.Add(false);
            m_listPlaneSliceShaderFlag.Add(0.0f);
        }

        var nor = m_Box.GetComponent<MeshFilter>().mesh;

    }

    private void Update()
    {
        m_BoxSlicer.transform.GetChild(0).position = new Vector3(m_MaxX, 0.0f, 0.0f);
        m_BoxSlicer.transform.GetChild(1).position = new Vector3(0.0f, m_MaxY, 0.0f);
        m_BoxSlicer.transform.GetChild(2).position = new Vector3(0.0f, 0.0f, m_MaxZ);
        m_BoxSlicer.transform.GetChild(3).position = new Vector3(m_MinX, 0.0f, 0.0f);
        m_BoxSlicer.transform.GetChild(4).position = new Vector3(0.0f, m_MinY, 0.0f);
        m_BoxSlicer.transform.GetChild(5).position = new Vector3(0.0f, 0.0f, m_MinZ);

        Shader.SetGlobalVector("g_MaxXPos", m_BoxSlicer.transform.GetChild(0).position);
        Shader.SetGlobalVector("g_MaxYPos", m_BoxSlicer.transform.GetChild(1).position);
        Shader.SetGlobalVector("g_MaxZPos", m_BoxSlicer.transform.GetChild(2).position);
        Shader.SetGlobalVector("g_MinXPos", m_BoxSlicer.transform.GetChild(3).position);
        Shader.SetGlobalVector("g_MinYPos", m_BoxSlicer.transform.GetChild(4).position);
        Shader.SetGlobalVector("g_MinZPos", m_BoxSlicer.transform.GetChild(5).position);
        Shader.SetGlobalVector("g_MaxXNor", m_BoxSlicer.transform.GetChild(0).up);
        Shader.SetGlobalVector("g_MaxYNor", m_BoxSlicer.transform.GetChild(1).up);
        Shader.SetGlobalVector("g_MaxZNor", m_BoxSlicer.transform.GetChild(2).up);
        Shader.SetGlobalVector("g_MinXNor", m_BoxSlicer.transform.GetChild(3).up);
        Shader.SetGlobalVector("g_MinYNor", m_BoxSlicer.transform.GetChild(4).up);
        Shader.SetGlobalVector("g_MinZNor", m_BoxSlicer.transform.GetChild(5).up);

        for (int i = 0; i < PLANE_SLICER_NUM; i++)
        {
            if (m_listPlaneSliceFlag[i])
            {
                if (!m_listPlaneSlicer[i].gameObject.activeSelf)
                    m_listPlaneSlicer[i].gameObject.SetActive(true);

                m_listPlaneSlicerPositions[i] = m_listPlaneSlicer[i].position;
                m_listPlaneSlicerNormals[i] = m_listPlaneSlicer[i].up;
                m_listPlaneSliceShaderFlag[i] = Convert.ToInt32(m_listPlaneSliceFlag[i]);
            }
            else
            {
                if (m_listPlaneSlicer[i].gameObject.activeSelf)
                    m_listPlaneSlicer[i].gameObject.SetActive(false);
            }
        }

        Shader.SetGlobalVectorArray("g_PlanePositions", m_listPlaneSlicerPositions);
        Shader.SetGlobalVectorArray("g_PlaneNormals", m_listPlaneSlicerNormals);
        Shader.SetGlobalFloatArray("g_PlaneSliceFlag", m_listPlaneSliceShaderFlag);
    }
}
