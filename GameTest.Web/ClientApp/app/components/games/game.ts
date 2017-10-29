import { HubConnection } from '@aspnet/signalr-client';
import { inject } from 'aurelia-framework';

export class Player {
    connectionId: string;
    name: string;
    score: number;
}

export abstract class Round<TAnswer> {
    roundNumber: number;
    expression: string;
    winner: Player;
    givenAnswer: TAnswer;

    timeLeft: number;
}

export abstract class Game<TAnswer, TRound extends Round<TAnswer>>
{
    protected hubConnection: HubConnection;
    protected connectionPromise?: Promise<void>;

    public player: Player;
    public playerList: Player[];

    public rounds: TRound[] = [];
    public roundTemplate: string;

    public messages: string[] = [];

    constructor(hubName: string) {
        this.hubConnection = new HubConnection('/' + hubName);
        this.setupHubEvents();
    }

    setupHubEvents() {

        this.hubConnection.on('ServerFull', () => {
            alert("Only up to 10 players allowed, try again latter");
            window.location.replace('/#');
        });

        this.hubConnection.on('SetPlayersList', (playerList: Player[]) => {
            this.playerList = playerList;
        });

        this.hubConnection.on('PlayerCreated', (player: Player) => {
            this.player = player;
        });
        
        this.hubConnection.on('AddRounds', (rounds: TRound[]) => {
            this.rounds = this.rounds.concat(rounds);

            while (this.rounds.length > 15) {
                this.rounds.shift();
            }

        });

        this.hubConnection.on('UpdateRound', (round: TRound) => {

            var index = this.rounds.findIndex(r => r.roundNumber == round.roundNumber);
            var roundToUpdate = this.rounds[index];
            roundToUpdate.winner = round.winner;
        });

        this.hubConnection.on('Message', (message: string) => {
            this.messages.unshift(message);
        });

        this.hubConnection.on('StartRoundTimer', (roundNumber: number) => {
            var index = this.rounds.findIndex(r => r.roundNumber == roundNumber);
            var round = this.rounds[index];
            
            setTimeout(() => { round.timeLeft = 2; }, 1000);
            setTimeout(() => { round.timeLeft = 1; }, 2000);
            setTimeout(async () => { await this.hubConnection.invoke("RoundTimeout", roundNumber); }, 3000);
        });
    }

    async giveAnswer(round: TRound, answer: TAnswer) {
        round.givenAnswer = answer;
        await this.hubConnection.invoke("GiveAnswer", round.roundNumber, answer);
    }

    activate() {
        this.connectionPromise = this.hubConnection.start();
    }

    deactivate() {
        this.connectionPromise = undefined;
        this.hubConnection.stop();
    }
}