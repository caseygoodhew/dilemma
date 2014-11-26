using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    public static class PointConfigurationInitialization
    {
        internal static void Seed(DilemmaContext context)
        {
            var pointTypes = Enum.GetValues(typeof(PointType)).Cast<PointType>().OrderBy(x => (int)x).Select(
                x =>
                    {
                        var member = x.GetType().GetMember(x.ToString()).Single();

                        var display =
                            member.GetCustomAttributes(typeof(DisplayAttribute), false)
                                .Cast<DisplayAttribute>()
                                .Single();

                        var value =
                            member.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                                .Cast<DefaultValueAttribute>()
                                .Single();

                        return new PointConfiguration
                                   {
                                       PointType = x,
                                       Name = display.Name,
                                       Description = display.Description,
                                       Points = (int)value.Value
                                   };
                    });

            foreach (var pointType in pointTypes.Where(pointType => !context.PointConfigurations.Any(x => x.PointType == pointType.PointType)))
            {
                context.PointConfigurations.Add(pointType);
            }
        }
    }
}
