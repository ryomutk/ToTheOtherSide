using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class SectorButtonField:ButtonField
{
    protected override DataIndexer ExtractIndexer()
    {
        var sectors = entity.GetData<SalvageValuable<ISalvageData>>(0).value as DataIndexer;

        var root = sectors.GetData<SectorData>(0).rootStep;
        var datas = new List<SectorStepData>();

        root.GetAllOffsprings(ref datas);
        
        return new DataIndexer(datas.ToList<ISalvageData>());
    }
}