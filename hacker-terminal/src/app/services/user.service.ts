import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { LeaderboardEntry } from '../models/leaderboard-entry.model';
import { GameSessionHistory } from '../models/game-session.model';
import { PlayerStats } from '../models/player-stats.model';
import { UserProfile } from '../models/user-profile.model';
import { PlayerRank } from '../models/player-rank.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl = `${environment.apiUrl}/Users`;
  private gameSessionsUrl = `${environment.apiUrl}/GameSessions`;

  constructor(private http: HttpClient) { }

  getMyProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.usersUrl}/me`);
  }

  getMyStats(): Observable<PlayerStats> {
    return this.http.get<PlayerStats>(`${this.gameSessionsUrl}/my-stats`);
  }

  getMyHistory(page: number = 1): Observable<GameSessionHistory[]> {
    return this.http.get<GameSessionHistory[]>(
      `${this.gameSessionsUrl}/my-history?page=${page}`
    );
  }

  getLeaderboard(
    period: 'all' | 'week' | 'month' = 'all'
  ): Observable<LeaderboardEntry[]> {
    return this.http.get<LeaderboardEntry[]>(
      `${this.gameSessionsUrl}/leaderboard`,
      { params: { period } }
    );
  }

  getMyRank(
    period: 'all' | 'week' | 'month' = 'all'
  ): Observable<PlayerRank> {
    return this.http.get<PlayerRank>(
      `${this.gameSessionsUrl}/my-rank`,
      { params: { period } }
    );
  }

}
