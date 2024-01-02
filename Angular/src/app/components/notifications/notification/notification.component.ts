import { Component, computed, Signal, Input } from '@angular/core';
import { Notification } from 'src/app/shared/classes/notification/notification';
import { ReminderNotificationData } from 'src/app/shared/classes/notification/notification-data/reminder-notification-data';
import { NotificationType } from 'src/app/shared/enums/notification-type';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
    selector: 'app-notification',
    templateUrl: './notification.component.html',
    styleUrls: ['./notification.component.scss'],
  })
  export class NotificationComponent {
    @Input() notification: Notification = this.notificationService.dummyNotification;
    notificationsCount: Signal<number> = computed(() => this.notifications().length);
    showNotifications: Signal<boolean> = computed(() => this.notificationsCount() > 0);
    notifications: Signal<Notification[]> = computed(() => this.notificationService.notifications$());
    readonly MILLIS_PER_SECOND = 1000;
    readonly MILLIS_PER_MINUTE = this.MILLIS_PER_SECOND * 60;
    readonly MILLIS_PER_HOUR = this.MILLIS_PER_MINUTE * 60;

    constructor(private notificationService: NotificationService){
    }

    ngOnInit(): void{

    }

    getNotificationTextByType(notification: Notification): string{
      switch (notification.notificationType){
        case (NotificationType.ReminderNotification): {
          const notificationData: ReminderNotificationData = notification.notificationData as ReminderNotificationData;
          return `Task ${notificationData.toDoItemName} on list ${notificationData.toDoListName} is in ${this.getDeadline(notificationData.deadline)}`;
        }
      }
      return "";
    }

    getDeadline(deadline: Date): string{
      const diffInMinutes: number = (deadline.getTime() - Date.now())/this.MILLIS_PER_MINUTE;
      return  diffInMinutes > 59 ? `${diffInMinutes} minutes` : `an hour`;
    }
  }