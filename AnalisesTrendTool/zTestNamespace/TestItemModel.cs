using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AnalisesTrendTool.zTestNamespace
{
    public class TestItemModel
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }

    public class TestItems : ObservableCollection<TestItemModel>
    {

    }

    public class TestItemsList : ObservableCollection<TestItems>
    {

    }
}
