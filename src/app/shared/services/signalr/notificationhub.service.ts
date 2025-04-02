import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class NotificationhubService {
  private hubConnection: any;

  private readonly baseUrl =
    // 'http://localhost:58585/notifications?access_token=';
    'http://173.249.54.173/notifications?access_token=';

  public userNotifications$ = new BehaviorSubject<string | null>(null);

  constructor() {}
  startConnection(userToken: any): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + userToken, {
        transport:
          signalR.HttpTransportType.WebSockets |
          signalR.HttpTransportType.ServerSentEvents,
      })
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connection Started'))
      .catch((err: any) =>
        console.error('Error while starting connection: ', err)
      );

    this.addListeners();
  }
  private addListeners(): void {
    if (!this.hubConnection) return;

    this.hubConnection.on('ReceiveMessage', (message: any) => {
      console.log('User Notification:', message);
      this.userNotifications$.next(message);
    });
  }
  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .then(() => console.log('SignalR Connection Stopped'))
        .catch((err: any) =>
          console.error('Error while stopping connection: ', err)
        );
    }
  }
}
