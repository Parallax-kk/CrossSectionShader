using mattatz.TransformControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrossSection : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_listBoxSlicer = new List<Transform>();

    [SerializeField]
    private List<Vector4> m_listSlicerPositions = new List<Vector4>();

    [SerializeField]
    private List<Vector4> m_listSlicerNormals = new List<Vector4>();

    [SerializeField]
    private List<TransformControl> m_listTransformControl = new List<TransformControl>();

    private void Update()
    {
        Shader.SetGlobalVector("g_MaxXPos", m_listBoxSlicer[0].position);
        Shader.SetGlobalVector("g_MaxYPos", m_listBoxSlicer[1].position);
        Shader.SetGlobalVector("g_MaxZPos", m_listBoxSlicer[2].position);
        Shader.SetGlobalVector("g_MinXPos", m_listBoxSlicer[3].position);
        Shader.SetGlobalVector("g_MinYPos", m_listBoxSlicer[4].position);
        Shader.SetGlobalVector("g_MinZPos", m_listBoxSlicer[5].position);
        Shader.SetGlobalVector("g_MaxXNor", m_listBoxSlicer[0].up);
        Shader.SetGlobalVector("g_MaxYNor", m_listBoxSlicer[1].up);
        Shader.SetGlobalVector("g_MaxZNor", m_listBoxSlicer[2].up);
        Shader.SetGlobalVector("g_MinXNor", m_listBoxSlicer[3].up);
        Shader.SetGlobalVector("g_MinYNor", m_listBoxSlicer[4].up);
        Shader.SetGlobalVector("g_MinZNor", m_listBoxSlicer[5].up);

        for (int i = 0; i < m_listBoxSlicer.Count; i++)
        {
            m_listSlicerPositions[i] = m_listBoxSlicer[i].position;
            m_listSlicerNormals[i] = m_listBoxSlicer[i].up;

            Shader.SetGlobalVectorArray("g_Positions", m_listSlicerPositions);
            Shader.SetGlobalVectorArray("g_Normals", m_listSlicerNormals);
            m_listTransformControl[i].Control();
        }
    }
}