import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { LeaderboardEntry } from '../../models/leaderboard-entry.model';
import { PlayerRank } from '../../models/player-rank.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-leaderboard',
  standalone: true,
  imports: [],
  templateUrl: './leaderboard.component.html',
  styleUrl: './leaderboard.component.css'
})
export class LeaderboardComponent implements OnInit {
  public leaderboard: LeaderboardEntry[] = [];
  public myNeighbors: LeaderboardEntry[] = [];
  public myRank: PlayerRank | null = null;
  public selectedPeriod: 'all' | 'week' | 'month' = 'all';
  public isLoading = true;
  public errorMessage = '';

  constructor(
    private userService: UserService,
    private router: Router,
    private changeDetector: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadLeaderboard();
  }

  public loadLeaderboard(): void {
    this.isLoading = true;
    this.errorMessage = '';

    forkJoin({
      leaderboard: this.userService.getLeaderboard(this.selectedPeriod),
      myRank: this.userService.getMyRank(this.selectedPeriod),
      myNeighbors: this.userService.getMyNeighbors(this.selectedPeriod)
    }).subscribe({
      next: (data) => {
        this.leaderboard = data.leaderboard;
        this.myRank = data.myRank;
        this.myNeighbors = data.myNeighbors;
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Unable to load the leaderboard.';
        this.changeDetector.markForCheck();
      }
    });
  }

  public onPeriodChange(event: Event): void {
    const period = (event.target as HTMLSelectElement).value;

    if (period === 'all' || period === 'week' || period === 'month') {
      this.selectedPeriod = period;
      this.loadLeaderboard();
    }
  }

  public getOperatorLevel(totalXp: number): number {
    return Math.floor(totalXp / 1000) + 1;
  }

  public goBack(): void {
    this.router.navigate(['/home']);
  }
}
