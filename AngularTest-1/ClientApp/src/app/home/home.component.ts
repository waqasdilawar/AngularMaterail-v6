import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  checked = false;
  indeterminate = false;
  labelPosition = 'after';
  disabled = false;
}
