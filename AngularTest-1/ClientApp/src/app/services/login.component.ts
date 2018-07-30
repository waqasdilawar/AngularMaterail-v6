import { Component, OnInit, EventEmitter } from '@angular/core';
import { AppUser } from './app-user';
import { AppUserAuth } from './app-user-auth';
import { SecurityService } from './security.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AnimationHandlingService } from './animation-handling.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    user: AppUser = new AppUser();
    securityObject: AppUserAuth = null;
    returnUrl: string;
    switchstate = 'original';
    constructor(private securityService: SecurityService,
        private route: ActivatedRoute,
        private router: Router,
        private animationSvc: AnimationHandlingService) { }

    ngOnInit() {
        this.returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
    }

    login(state: string) {
        //For Animation
        this.switchstate = state;
        console.log(state);
        this.animationSvc.switchstate = state;

        this.securityService.login(this.user)
            .subscribe(resp => {
                this.securityObject = resp;
                if (this.returnUrl) {
                    this.router.navigateByUrl(this.returnUrl);
                }
            },
                () => {
                    // Initialize security object to display error message
                    this.securityObject = new AppUserAuth();
                });
    }
}
