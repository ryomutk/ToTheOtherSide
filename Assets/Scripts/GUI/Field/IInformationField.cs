using System.Collections;

public interface IInformationField
{
    SmallTask LoadDataAsync();
    void UnloadData();
}