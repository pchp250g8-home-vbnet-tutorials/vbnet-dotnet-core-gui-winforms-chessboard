Imports SlimDX
Imports SlimDX.Direct3D9

Public Class Form1
    Private m_oD3D As Direct3D
    Private m_oD3D_Device As Device
    Private m_d3dpp As PresentParameters

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If (Not InitializeDirect3D()) Then
            MessageBox.Show("Failed Loading DirectX", "Error")
            Application.Exit()
            Return
        End If
        SetStyle(ControlStyles.Opaque, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DestroyDirect3D()
    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        RenderScene()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If (m_oD3D Is Nothing) Then Return
        If (m_oD3D_Device Is Nothing) Then Return
        If (ClientSize.Width > 0 AndAlso ClientSize.Height > 0) Then
            m_d3dpp.BackBufferWidth = ClientSize.Width
            m_d3dpp.BackBufferHeight = ClientSize.Height
            m_oD3D_Device.Reset(m_d3dpp)
        End If
        Invalidate()
    End Sub
    Private Function InitializeDirect3D() As Boolean
        Dim bResult As Boolean = False
        m_d3dpp = New PresentParameters()
        m_d3dpp.SwapEffect = SwapEffect.Discard
        m_d3dpp.Windowed = True
        m_d3dpp.EnableAutoDepthStencil = False
        m_d3dpp.BackBufferWidth = ClientSize.Width
        m_d3dpp.BackBufferHeight = ClientSize.Height
        Try
            m_oD3D = New Direct3D()
            Dim nAdapter = m_oD3D.Adapters.DefaultAdapter.Adapter
            Dim d3ddm = m_oD3D.GetAdapterDisplayMode(nAdapter)
            m_d3dpp.BackBufferFormat = d3ddm.Format
            m_oD3D_Device = New Device _
            (
                m_oD3D, nAdapter,
                DeviceType.Hardware,
                Handle,
                CreateFlags.SoftwareVertexProcessing,
                m_d3dpp
            )
            bResult = Not (m_oD3D_Device Is Nothing)
        Catch
            bResult = False
        End Try
        Return bResult
    End Function
    Private Sub DestroyDirect3D()
        If Not (m_oD3D_Device Is Nothing) Then
            m_oD3D_Device.Dispose()
        End If
        If Not (m_oD3D! Is Nothing) Then
            m_oD3D.Dispose()
        End If
    End Sub
    Private Sub RenderScene()
        Dim k As Integer = 0
        Dim Width As Integer = ClientSize.Width / 8
        Dim Height As Integer = ClientSize.Height / 8
        Dim d3dColor1 As Color4 = New Color4(Color.Aqua)
        Dim d3dColor2 As Color4 = New Color4(Color.Brown)
        Dim rects(32) As Rectangle
        If (m_oD3D Is Nothing) Then Return
        If (m_oD3D_Device Is Nothing) Then Return
        Dim d3dResult As Result = m_oD3D_Device.TestCooperativeLevel()
        If (d3dResult = ResultCode.DeviceLost) Then Return
        If (d3dResult = ResultCode.DeviceNotReset) Then
            m_oD3D_Device.Reset(m_d3dpp)
            Invalidate()
        End If
        m_oD3D_Device.Clear(ClearFlags.Target, d3dColor1, 0.0F, 0)
        For i As Integer = 0 To 7
            For j As Integer = i Mod 2 To 7 Step 2
                Dim Left As Integer = i * Width
                Dim Top As Integer = j * Height
                rects(k) = New Rectangle(Left, Top, Width, Height)
                k += 1
            Next j
        Next i
        m_oD3D_Device.Clear(ClearFlags.Target, d3dColor2, 0.0F, 0, rects)
        m_oD3D_Device.BeginScene()
        m_oD3D_Device.EndScene()
        m_oD3D_Device.Present()
    End Sub

End Class
