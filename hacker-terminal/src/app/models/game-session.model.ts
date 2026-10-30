export interface GameSessionStartResponse {
  sessionId: number;
  scenarioId: number;
  challengePrompt: string;
  scenarioTitle: string;
  startedAt: string;
  timeLimitSeconds: number;
}

export interface SubmitAnswerResponse {
  isCorrect: boolean;
  isCompleted: boolean;
  message: string;
  score?: number;
  xpReward?: number;
  timeTakenSeconds?: number;
}

export interface GameSessionHistory {
  scenarioTitle: string;
  score: number;
  xpReward: number;
  timeTakenSeconds: number;
  isSuccessful: boolean;
  finishedAt: string | null;
}
