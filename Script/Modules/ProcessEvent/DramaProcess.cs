public class DramaProcess : IProcessEvent
{
    public ProcessType processType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    private string m_dramaKey;

    public DramaProcess(string dramaKey)
    {
        m_dramaKey = dramaKey;
    }
    public bool Execute()
    {
        // �o�e�ƥ�A�t�X�@��
        processType = ProcessType.Processing;
        return true;
    }

    public void Inilization()
    {
        m_dramaKey = string.Empty;
        processType = ProcessType.None;
    }
}
