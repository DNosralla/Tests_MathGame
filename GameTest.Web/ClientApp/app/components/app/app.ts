import { Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([{
            route: ['', 'home'],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'
        },
        {
            route: ['rightOrWrong'],
            name: 'rightOrWrong',
            moduleId: PLATFORM.moduleName('../games/rightOrWrong'),
            title: 'Righ Or Wrong'
        },
        {
            route: ['guessTheAnswer'],
            name: 'guessTheAnswer',
            moduleId: PLATFORM.moduleName('../games/guessTheAnswer'),
            title: 'Guess The Answer'
        }
        ]);

        this.router = router;
    }
}
