using System.Linq;
using System.Reflection;
using FubuCore;
using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public class TestModel
    {
        public bool? LikesBlue { get; set; }
        public bool? LikesGreen { get; set; }
        public bool? LikesRed { get; set; }

        public override string ToString()
        {
            return GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => "{0}={1}".ToFormat(x.Name, x.GetValue(this, null) ?? "null"))
                .Join(",");
        }
    }
}