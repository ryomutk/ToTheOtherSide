//永続化するデータ
public interface IPermanentData:ISalvageData
{
    //dataで上書きする
    bool UpdateData(ISalvageData data);
}