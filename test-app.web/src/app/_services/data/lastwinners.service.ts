import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs/Rx';
import { WebsocketService } from '../websocket.service';

import { environment } from '../../../environments/environment';
import { LastWinner } from '../../layout/lastwinners/lastwinners.model';

@Injectable()
export class LastWinnersService {
	public messages: Subject<LastWinner>;

	constructor(wsService: WebsocketService) {
		this.messages = <Subject<LastWinner>>wsService
			.connect(environment.endPointWSLastWinners)
			.map((response: MessageEvent): LastWinner => {
				let data = JSON.parse(response.data);
				return data;
			});
	}
}
