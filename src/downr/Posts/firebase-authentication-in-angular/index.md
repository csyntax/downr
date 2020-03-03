---
title: Firebase Authentication in Angular
date: 05-08-2018
tags: Angular, JavaScript
---

![Firebase Authentication in Angular](media/header.png)

Firebase provides a very simple way to setup 
[authentication](https://firebase.google.com/docs/auth/) in your apps. 
Here we’ll explore how to setup a simple email/password signup and authentication workflow for 
**Angular 2+** apps using the 
[AngularFire2](https://github.com/angular/angularfire2) library.

The first step will be to create a new Firebase project and enable the *Email/Password* sign-in method under the _Authentication_ section of the Firebase console.

## Installing and Setting Up AngularFire2

Let’s get started by installing the necessary packages using npm. This will add the Firebase SDK, AngularFire2 and a promise-polyfill dependency to your project:

`npm install firebase angularfire2 --save`

Now let’s add our Firebase API and project credentials to the `firebaseConfig.ts` file of our project. You’ll find these values in your Firebase console if you click on Add Firebase to your web app:

```typescript
export const firebaseConfig = {
    apiKey: "YOUR API KEY",
    authDomain: "YOUR AUTH DOMAIN",
    databaseURL: "YOUR DB URL",
    projectId: "YOUR PROJECT ID",
    storageBucket: "YOUR STORAGE BUCKET",
    messagingSenderId: "YOUR SENDER ID"
};
```

Then we’ll configure our app module with *AngularFireModule* and *AngularFireAuthModule*. 
Notice also that we’re importing and providing an *AuthService*.

`app.module.ts`

```typescript
// ...

import { AngularFireModule } from 'angularfire2';
import { environment } from '../environments/environment';
import { AngularFireAuthModule } from 'angularfire2/auth';

import { AuthService } from './auth.service';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        // ...
        AngularFireModule.initializeApp(environment.firebase),
        AngularFireAuthModule
    ],
    providers: [AuthService],
    bootstrap: [AppComponent]
})
export class AppModule { }
```

## Creating the Auth Service

Our service will be a central place that allows us to login, signup or logout users, so we’ll define methods for these 3 actions. All the authentication logic will be in the service, which will allow us to create components that don’t need to implement any auth logic and will help keep our components simple.

You can create the skeleton for the service using the Angular CLI using this command: 
`ng gènerate service auth`

And here’s the implementation of the service, making use of **AngularFireAuth**.

```typescript
import { Injectable } from '@angular/core';
import { AngularFireAuth } from 'angularfire2/auth';
import { User } from 'firebase/app'
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthService {
    user: Observable<firebase.User>;

    constructor(private firebaseAuth: AngularFireAuth) {
        this.user = firebaseAuth.authState;
    }

    public signup(email: string, password: string): void {
        this.firebaseAuth
            .auth
            .createUserWithEmailAndPassword(email, password)
            .then(value => {
                console.log(`Success! ${value}`);
            })
            .catch(err => {
                console.log(`Something went wrong: ${err.message}`);
            });    
    }

    public signin(email: string, password: string): void {
        this.firebaseAuth
            .auth
            .signInWithEmailAndPassword(email, password)
            .then(value => {
                console.log('You are signed in!');
            })
            .catch(err => {
                console.log(`Something went wrong: ${err.message}`);
            });
    }

    public signout(): void {
        this.firebaseAuth.auth.signOut();
    }
}
```

## Component Class and Template

Now that our auth service is in place, it’s really simple to create 
a component that allows for sign in, sign up or sing out:

```typescript
import { Component } from '@angular/core';
import { AuthService } from './auth.service';

@Component({ ... })
export class AppComponent {
    public email: string;
    public password: string;

    constructor(public authService: AuthService) {}

    public signUp(): void {
        this.authService.signup(this.email, this.password);

        this.email = '';
        this.password = '';
    }

    public signIn(): void {
        this.authService.login(this.email, this.password);

        this.email = '';
        this.password = '';
    }

    public signOut(): void {
        this.authService.signout();
    }
}
```

We inject the service in the component’s constructor and then define local methods that call the equivalent methods on the auth service.
We inject the service with the public keyword instead of private so that we can access the service properties directly from the template too.

**The template is very simple and uses the async pipe on the authService’s user object to determine if there’s a logged-in user or not:**

```html
<h1 *ngIf="authService.user | async">
    Welcome {{ (authService.user | async)?.email }}!
</h1>

<div *ngIf="!(authService.user | async)">
    <input type="text" [(ngModel)]="email" placeholder="email">
    <input type="password" [(ngModel)]="password" placeholder="email">

    <button (click)="signUp()" [disabled]="!email || !password">Sign up</button>
    <button (click)="signIn()" [disabled]="!email || !password">Sign in</button>
</div>

<button (click)="signOut()" *ngIf="authService.user | async">Sign out</button>
```

Our input fields two-way bind to the email and password values in our component class using _ngModel_
and the banana in a box syntax.

Done! It's that simple to add authentication to your apps using Firebase authentication.