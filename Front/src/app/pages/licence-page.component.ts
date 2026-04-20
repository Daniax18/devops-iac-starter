import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-licence-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './licence-page.component.html',
  styleUrl: './licence-page.component.css',
})
export class LicencePageComponent {
  protected readonly clientName: string = environment.clientName;
  protected readonly clientSiege: string = environment.clientSiege;
  protected readonly appPort: string = environment.appPort;
  protected readonly year: number = new Date().getFullYear();
}
