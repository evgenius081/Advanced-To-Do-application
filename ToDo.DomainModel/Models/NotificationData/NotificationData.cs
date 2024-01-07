using System.Text.Json.Serialization;
using ToDo.DomainModel.Enums;
using ToDo.DomainModel.Extensions;

namespace ToDo.DomainModel.Models.NotificationData
{
    /// <summary>
    /// Abstract class for notification data.
    /// </summary>
    [JsonPolymorphic(
    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(ReminderNotificationData), typeDiscriminator: nameof(ReminderNotificationData))]
    public abstract class NotificationData
    {
    }
}
