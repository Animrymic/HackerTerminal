export interface Scenario {
  id: number;
  title: string;
  description: string;
  category: string;
  difficulty: string;
  challengePrompt: string;
  xpReward: number;
  timeLimitSeconds: number;
  isActive: boolean;
  createdAt: string;
}
