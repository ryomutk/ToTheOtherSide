using System.Collections;

public interface IInformationField
{
    ITask LoadDataAsync();
    void UnloadData();
}