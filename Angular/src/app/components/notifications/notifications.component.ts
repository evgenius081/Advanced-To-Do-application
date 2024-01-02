import { Component, computed, Signal } from '@angular/core';
import { Notification } from 'src/app/shared/classes/notification/notification';
import { HttpService } from "../../shared/services/http.service";
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
    selector: 'app-notifications',
    templateUrl: './notifications.component.html',
    styleUrls: ['./notifications.component.scss'],
  })
  export class NotificationsComponent {
    notificationsCount: Signal<number> = computed(() => this.notifications().length);
    showNotifications: Signal<boolean> = computed(() => this.notificationsCount() > 0);
    notifications: Signal<Notification[]> = computed(() => this.notificationService.notifications$());

    constructor(private httpService: HttpService, private notificationService: NotificationService){
    }

    ngOnInit(): void{
      this.notificationService.getNotifications();
    }
  }