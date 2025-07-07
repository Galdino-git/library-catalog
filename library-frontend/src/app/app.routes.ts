import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Signup } from './components/signup/signup';
import { BookRegister } from './components/book-register/book-register';
import { BookList } from './components/book-list/book-list';
import { Profile } from './components/profile/profile';
import { Home } from './components/home/home';

export const routes: Routes = [
    {
        path: '',
        component: Home
    },
    {
        path: 'login',
        component: Login
    },
    {
        path: 'signup',
        component: Signup
    },
    {
        path: 'register',
        component: BookRegister
    },
    {
        path: 'list',
        component: BookList
    },
    {
        path: 'profile',
        component: Profile
    }
];
