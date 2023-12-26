export interface TodoListWithStatistics {
  id: number;
  title: string;
  isArchived: boolean;
  itemsNotStarted: number;
  itemsInProcess: number;
  itemsCompleted: number;
  userID: number;
}
