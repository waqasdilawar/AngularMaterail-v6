import { Injectable, Inject, EventEmitter } from '@angular/core';

@Injectable()
export class AnimationHandlingService {
    switchstate = 'original';
    onLogIn = new EventEmitter();
}