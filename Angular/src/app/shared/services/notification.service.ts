import { Injectable, WritableSignal, signal } from "@angular/core";
import { Observable } from "rxjs";
import { Notification } from "../classes/notification/notification";
import { HttpService } from "./http.service";
import { TokenService } from "./token.service";
import { TokenType } from "../enums/token-type";
import { NotificationState } from "../enums/notification-state";
import { NotificationType } from "../enums/notification-type";

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
        notificationData: {}
    }

    constructor(private httpService: HttpService, private tokenService: TokenService){}

    getNotifications(): void{
        const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
        this.httpService.get<Notification[]>("notifications", token)
        .subscribe(items => this.notifications$.set(items));
    }
}