import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class NotificationhubService {
  private hubConnection!: signalR.HubConnection;
  private readonly baseUrl =
    // 'http://173.249.54.173/notifications?access_token=';
    'https://talentmesh.genericsolution.net/notifications?access_token=';
  // 'http://localhost:51027/notifications?access_token=';

  public userNotifications$ = new BehaviorSubject<string | null>(null);

  constructor() {}

  public connectionEstablished$ = new BehaviorSubject<boolean>(false);

  startConnection(userToken: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + userToken)
      .configureLogging(signalR.LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connection Started');
        this.addListeners();
        this.connectionEstablished$.next(true); // <-- Notify connection ready
      })
      .catch((err) => {
        this.connectionEstablished$.next(false);
      });
  }

  private addListeners(): void {
    if (!this.hubConnection) return;
    console.log('Registering SignalR event listeners...');

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
