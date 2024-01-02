import { NotificationState } from "../../enums/notification-state";
import { NotificationType } from "../../enums/notification-type";

export interface Notification{
    id: number;
    notificationState: NotificationState;
    notificationType: NotificationType;
    notificationData: object;
    sentAt: Date;
    recipientId: number
}