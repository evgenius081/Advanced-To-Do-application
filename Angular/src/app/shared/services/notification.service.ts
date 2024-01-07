import { Injectable, WritableSignal, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { Notification } from '../classes/notification/notification';
import { HttpService } from './http.service';
import { TokenService } from './token.service';
import { TokenType } from '../enums/token-type';
import { NotificationState } from '../enums/notification-state';
import { NotificationType } from '../enums/notification-type';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  notifications$: WritableSignal<Notification[]> = signal<Notification[]>([]);
  dummyNotification: Notification = {
    id: -1,
    notificationState: NotificationState.Created,
    notificationType: NotificationType.ReminderNotification,
    recipientId: -1,
    sentAt: new Date(),
    notificationData: {},
  };
  private hubConnection: signalR.HubConnection;

  constructor(
    private httpService: HttpService,
    private tokenService: TokenService
  ) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env['NG_APP_BACKEND_BASE_URL']}/notify`)
      .build();
    this.startConnection();
  }

 private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        this.addNotificationListener();
        console.log('Connection started');
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  private addNotificationListener = () => {
    this.hubConnection.on('item-notification', (data) => {
      this.notifications$.update((prevState) => [...prevState, data as Notification]);
      console.log(data);
    });
  }

  public getNotifications(): void {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    this.httpService
      .get<Notification[]>('notifications', token)
      .subscribe((items) => this.notifications$.set(items));
  }
}
