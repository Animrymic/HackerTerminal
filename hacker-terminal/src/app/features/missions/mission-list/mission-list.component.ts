import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { Scenario } from '../../../models/scenario.model';
import { SubmitAnswerResponse } from '../../../models/game-session.model';
import { ScenarioService } from '../../../services/scenario.service';

@Component({
  selector: 'app-mission-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './mission-list.component.html',
  styleUrl: './mission-list.component.css'
})
export class MissionListComponent implements OnInit, OnDestroy {
  public scenarios: Scenario[] = [];
  public difficultyFilter: 'ALL' | 'EASY' | 'MEDIUM' | 'HARD' = 'ALL';
  public missionSearch = '';
  public isLoading = false;
  public errorMessage = '';
  public activeChallengePrompt = '';
  public activeScenarioId: number | null = null;
  public activeSessionId: number | null = null;
  public isStartingScenarioId: number | null = null;
  public isSubmitting = false;
  public answer = '';
  public answerMessage = '';
  public gameResult: SubmitAnswerResponse | null = null;
  public secondsRemaining = 0;

  private countdownInterval: ReturnType<typeof setInterval> | null = null;

  constructor(
    private scenarioService: ScenarioService,
    private router: Router,
    private changeDetector: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadScenarios();
  }

  ngOnDestroy(): void {
    this.stopCountdown();
  }

  public loadScenarios(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.scenarioService.getScenarios().subscribe({
      next: (scenarios: Scenario[]) => {
        this.scenarios = scenarios;
        this.isLoading = false;
        this.changeDetector.markForCheck();
      },
      error: (error: { status?: number }) => {
        this.isLoading = false;
        this.errorMessage = error.status === 401
          ? 'Your session has expired. Please log in again.'
          : 'Unable to load missions. Please try again.';
        this.changeDetector.markForCheck();
      }
    });
  }

  public startMission(scenario: Scenario): void {
    this.stopCountdown();
    this.secondsRemaining = 0;
    this.activeScenarioId = scenario.id;
    this.activeChallengePrompt = '';
    this.activeSessionId = null;
    this.isStartingScenarioId = scenario.id;
    this.answer = '';
    this.answerMessage = '';
    this.gameResult = null;

    this.scenarioService.startSession(scenario.id).subscribe({
      next: (session) => {
        this.activeSessionId = session.sessionId;
        this.isStartingScenarioId = null;
        this.activeChallengePrompt = session.challengePrompt;
        this.secondsRemaining = session.timeLimitSeconds;
        this.answerMessage =
          `Mission started. You have ${session.timeLimitSeconds} seconds.`;
        this.startCountdown();
        this.changeDetector.markForCheck();
      },
      error: (error: { status?: number }) => {
        this.isStartingScenarioId = null;
        this.answerMessage = error.status === 401
          ? 'Your session has expired. Please log in again.'
          : 'Unable to start this mission. Please try again.';
        this.changeDetector.markForCheck();
      }
    });
  }

  public submitAnswer(): void {
    if (this.activeSessionId === null || !this.answer.trim() || this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.answerMessage = '';

    this.scenarioService
      .submitAnswer(this.activeSessionId, this.answer.trim())
      .subscribe({
        next: (result: SubmitAnswerResponse) => {
          this.gameResult = result;
          this.answerMessage = result.message;
          this.isSubmitting = false;
          this.answer = '';

          if (result.isCompleted) {
            this.activeSessionId = null;
            this.stopCountdown();
          }

          this.changeDetector.markForCheck();
        },
        error: (error: { status?: number }) => {
          this.isSubmitting = false;
          this.answerMessage = error.status === 401
            ? 'Your session has expired. Please log in again.'
            : 'Unable to submit your answer. Please try again.';
          this.changeDetector.markForCheck();
        }
      });
  }

  public goBack(): void {
    this.stopCountdown();
    this.router.navigate(['/home']);
  }

  private startCountdown(): void {
    this.stopCountdown();

    this.countdownInterval = setInterval(() => {
      if (this.secondsRemaining > 0) {
        this.secondsRemaining--;
      }

      if (this.secondsRemaining === 0) {
        this.stopCountdown();
        this.answerMessage =
          'Time is up. Submit your answer to finish the mission.';
      }

      this.changeDetector.markForCheck();
    }, 1000);
  }

  private stopCountdown(): void {
    if (this.countdownInterval !== null) {
      clearInterval(this.countdownInterval);
      this.countdownInterval = null;
    }
  }

  public get filteredScenarios(): Scenario[] {
    const search = this.missionSearch.trim().toLowerCase();

    return this.scenarios.filter(scenario => {
      const matchesDifficulty =
        this.difficultyFilter === 'ALL' ||
        scenario.difficulty.toUpperCase() === this.difficultyFilter;

      const searchableText =
        `${scenario.title} ${scenario.category} ${scenario.description}`
          .toLowerCase();

      const matchesSearch =
        search.length === 0 || searchableText.includes(search);

      return matchesDifficulty && matchesSearch;
    });
  }
}
