import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DatePipe } from '@angular/common';

import { PlayerStats } from '../../models/player-stats.model';
import { UserProfile } from '../../models/user-profile.model';
import { UserService } from '../../services/user.service';
import { GameSessionHistory } from '../../models/game-session.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  public profile: UserProfile | null = null;
  public stats: PlayerStats | null = null;
  public isLoading = true;
  public errorMessage = '';
  public history: GameSessionHistory[] = [];
  public achievementHistory: GameSessionHistory[] = [];
  public historyPage = 1;
  public hasOlderHistory = false;
  public isHistoryLoading = false;
  public historyErrorMessage = '';

  constructor(
    private userService: UserService,
    private router: Router,
    private changeDetector: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProfileData();
  }

  public loadProfileData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    forkJoin({
      profile: this.userService.getMyProfile(),
      stats: this.userService.getMyStats(),
      history: this.userService.getMyHistory()
    }).subscribe({
      next: (data) => {
        this.profile = data.profile;
        this.stats = data.stats;
        this.history = data.history;
        this.achievementHistory = data.history;
        this.historyPage = 1;
        this.hasOlderHistory = this.history.length === 50;
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Unable to load your profile.';
        this.changeDetector.markForCheck();
      }
    });
  }

  public loadOlderHistory(): void {
    if (this.hasOlderHistory && !this.isHistoryLoading) {
      this.loadHistoryPage(this.historyPage + 1);
    }
  }

  public loadNewerHistory(): void {
    if (this.historyPage > 1 && !this.isHistoryLoading) {
      this.loadHistoryPage(this.historyPage - 1);
    }
  }

  private loadHistoryPage(page: number): void {
    this.isHistoryLoading = true;
    this.historyErrorMessage = '';

    this.userService.getMyHistory(page).subscribe({
      next: (items) => {
        if (items.length === 0 && page > this.historyPage) {
          this.hasOlderHistory = false;
        } else {
          this.history = items;
          this.historyPage = page;
          this.hasOlderHistory = items.length === 50;
        }

        this.isHistoryLoading = false;
        this.changeDetector.markForCheck();
      },
      error: () => {
        this.historyErrorMessage = 'Unable to load this history page.';
        this.isHistoryLoading = false;
        this.changeDetector.markForCheck();
      }
    });
  }

  public get achievements(): {
    title: string;
    description: string;
    unlocked: boolean;
  }[] {
    const completed = this.stats?.completedMissions ?? 0;
    const totalXp = this.stats?.totalXp ?? 0;
    const bestScore = this.stats?.bestScore ?? 0;
    const hasRecentFlawlessRun = this.achievementHistory.some(
      entry =>
        entry.attemptsTracked &&
        entry.isSuccessful &&
        entry.incorrectAttempts === 0
    );

    return [
      {
        title: 'FIRST BREACH',
        description: 'Complete your first successful mission.',
        unlocked: completed >= 1
      },
      {
        title: 'FIELD OPERATIVE',
        description: 'Complete 10 successful missions.',
        unlocked: completed >= 10
      },
      {
        title: 'XP HUNTER',
        description: 'Earn 1,000 total XP.',
        unlocked: totalXp >= 1000
      },
      {
        title: 'SCORE BREAKER',
        description: 'Reach a best score of 100 or more.',
        unlocked: bestScore >= 100
      },
      {
        title: 'FLAWLESS OPERATOR',
        description: 'Complete a tracked mission with no incorrect answers.',
        unlocked: hasRecentFlawlessRun
      }
    ];
  }

  public goBack(): void {
    this.router.navigate(['/home']);
  }
}
