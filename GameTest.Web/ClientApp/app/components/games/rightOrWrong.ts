import { HubConnection } from '@aspnet/signalr-client';
import { inject } from 'aurelia-framework';
import { Player, Round, Game } from './game';

export class RightOrWrongGame extends Game<boolean, RightOrWrongRound>{
    constructor() {
        super("rightOrWrong");
        this.roundTemplate = "RightOrWrong";
    }
}

export class RightOrWrongRound extends Round<boolean> {
    public template: string = "teste";
}

