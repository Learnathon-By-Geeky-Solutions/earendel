// import { Injectable } from '@angular/core';
// import { BehaviorSubject } from 'rxjs';
// import * as signalR from '@microsoft/signalr';
// import { languages } from 'prismjs';

// @Injectable({
//   providedIn: 'root',
// })
// export class CodeService {
//   private hubConnection: any;
//   // private baseUrl = 'http://localhost:5007/codeHub';
//   public userNotifications$ = new BehaviorSubject<string | null>(null);

//   constructor(private categoryService: CategoryService) {}

//   startConnection(): void {
//     this.hubConnection = new signalR.HubConnectionBuilder()
//       .withUrl(this.baseUrl, {
//         transport:
//           signalR.HttpTransportType.WebSockets |
//           signalR.HttpTransportType.ServerSentEvents,
//       })
//       .configureLogging(signalR.LogLevel.Information)
//       .withAutomaticReconnect()
//       .build();

//     this.hubConnection
//       .start()
//       .then(() => console.log('SignalR Connection Started'))
//       .catch((err: any) =>
//         console.error('Error while starting connection: ', err)
//       );

//     this.addListeners();
//   }
//   private addListeners(): void {
//     if (!this.hubConnection) return;

//     // this.hubConnection.on('ReceiveMessage', (message: any) => {
//     //   console.log('User Notification:', message);
//     //   this.userNotifications$.next(message);
//     // });
//   }

//   public sendCodeUpdate(code: string, language: string) {
//     this.hubConnection
//       .invoke('SendCodeUpdate', code, language)
//       .catch((err: any) => console.error(err));
//   }

//   // Listen for code updates from other clients
//   public listenForCodeUpdates(
//     callback: (code: string, language: string) => void
//   ) {
//     // this.hubConnection.on('ReceiveCodeUpdate', (code: string, language: string) => {
//     this.hubConnection.on('ReceiveCodeUpdate', (data: any) => {
//       console.log('language', data.language);
//       callback(data.code, data.language);
//     });
//   }

//   stopConnection(): void {
//     if (this.hubConnection) {
//       this.hubConnection
//         .stop()
//         .then(() => console.log('SignalR Connection Stopped'))
//         .catch((err: any) =>
//           console.error('Error while stopping connection: ', err)
//         );
//     }
//   }
// }
