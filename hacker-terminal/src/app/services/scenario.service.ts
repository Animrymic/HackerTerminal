import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, timeout } from 'rxjs';

import { environment } from '../../environments/environment';
import { Scenario } from '../models/scenario.model';
import {
  GameSessionStartResponse,
  SubmitAnswerResponse
} from '../models/game-session.model';

@Injectable({
  providedIn: 'root'
})
export class ScenarioService {
  private apiUrl = `${environment.apiUrl}/Scenarios`;
  private gameSessionsUrl = `${environment.apiUrl}/GameSessions`;

  constructor(private http: HttpClient) { }

  getScenarios(): Observable<Scenario[]> {
    return this.http.get<Scenario[]>(this.apiUrl).pipe(
      timeout({ first: 10000 })
    );
  }

  startSession(scenarioId: number): Observable<GameSessionStartResponse> {
    return this.http.post<GameSessionStartResponse>(
      `${this.gameSessionsUrl}/${scenarioId}/start`,
      {}
    );
  }

  submitAnswer(
    sessionId: number,
    answer: string
  ): Observable<SubmitAnswerResponse> {
    return this.http.post<SubmitAnswerResponse>(
      `${this.gameSessionsUrl}/${sessionId}/submit`,
      { answer }
    );
  }
}
