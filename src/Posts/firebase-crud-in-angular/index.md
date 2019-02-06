---
title: Firebase CRUD Operations in Angular
slug: firebase-crud-in-angular
categories: Angular, JavaScript
date: 20-08-2018
---

![Firebase CRUD Operations in Angular](media/header.png)

Now let’s explore how to get started with the realtime database in *Angular* applications using the *AngularFire2* library. 
Firebase makes it very easy to get up and running very quickly with populating and performing operations on the database.

# Database and basic creation

First you’ll want to import **AngularFireDatabase** and **FirebaseListObservable** as well as inject the former in your constructor: `app.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { AngularFireDatabase, FirebaseListObservable } from 'angularfire2/database';

export class AppComponent implements OnInit {
    public papers: Observable<any[]>;

    constructor(private af: AngularFireDatabase) {}

    ngOnInit() {
        // ...
    }

    addPaper(value: string): void {
        // ...
    }

    deletePaper(paper: any): void {
        // ...
    }

    updatePaper(paper: any, newPaper: string): void {
        // ...
    }
}
```

You’ll also want to make sure that **AngularFireDatabase** is provided in `app.module.ts`.

```typescript
// ...

import { AngularFireModule } from 'angularfire2';
import { AngularFireDatabase } from 'angularfire2/database';
import { firebaseConfig } from '../firebaseConfig';

@NgModule({
    declarations: [AppComponent],
    imports: [
        // ...
        AngularFireModule.initializeApp(firebaseConfig),
        AngularFireAuthModule,
        AngularFireStorageModule,
        AngularFireDatabaseModule
    ],
    providers: [AngularFireDatabase],
    bootstrap: [AppComponent]
})
export class AppModule { }
```

## Reading papers from database

Simply declare a class property of type FirebaseListObservable and get the */papers* node from your Firebase database with **AngularFireDatabase.list** in the *OnInit* lifecycle hook:

```typescript
papers: Observable<any[]>;

ngOnInit() {
    this.papers = this.af.list('/papers').valueChanges();
}
```

You can unwrap the observable to display the todo items in your template using the async pipe like this:

```html
<ul>
    <li *ngFor="let paper of (papers | async)">
        {{paper.content}}
    </li>
</ul>
```

## Creating paper

Adding a new paper item is really easy, just call push on your **Observable** instance:

```typescript
addPaper(value: string): void {
    this.afDatabase.list('/papers').push({ content: value });
}
```

## Updating paper

Here's how you would toggle the completed state of a __paper__.

```typescript
updatePaper(paper: any, newValue: any): void {
    this.af.object('/papers/' + paper.$key).update({ content: newValue });
}
```

## Deleting paper

Deleting an item is just as easy as updating it:

```typescript
deletePaper(paper: any): void {
    this.af.object('/papers/' + paper.$key).remove();
}
```

### That is all

**Note that __set__, __update__ and __remove__ return a promise to you can chain __then/catch__ calls to let you act on a successful operation or an error.**