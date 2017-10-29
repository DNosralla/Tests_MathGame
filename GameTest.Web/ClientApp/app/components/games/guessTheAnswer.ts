import { HubConnection } from '@aspnet/signalr-client';
import { inject } from 'aurelia-framework';
import { Player, Round, Game } from './game';

export class GuessTheAnswerGame extends Game<number, GuessTheAnswerRound>{
    
    constructor() {
        super("guessTheAnswer");
        this.roundTemplate = "GuessTheAnswer";
    }

    async giveAnswer(round: GuessTheAnswerRound, answer: number) {
        if (isNaN(answer)) {
            round.inputAnswer = 0;
        } else {
            round.givenAnswer = answer;
            await this.hubConnection.invoke("GiveAnswer", round.roundNumber, answer);
        }
    }
}

export class GuessTheAnswerRound extends Round<number> {
    inputAnswer: number;
}

