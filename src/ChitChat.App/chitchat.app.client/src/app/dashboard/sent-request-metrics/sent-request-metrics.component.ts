import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from 'src/app/api/services/auth.service';
import { FriendshipService } from 'src/app/api/services/friendship.service';
import { ChartConfiguration, ChartType } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { FriendshipRequestStatsDto } from 'src/app/api/models/friendship-request-stats-dto';

@Component({
  selector: 'app-sent-request-metrics',
  standalone: true,
  imports: [CommonModule, NgChartsModule],
  templateUrl: './sent-request-metrics.component.html',
  styleUrls: ['./sent-request-metrics.component.css']
})
export class SentRequestMetricsComponent implements OnInit {
  sentRequestStats: FriendshipRequestStatsDto[] = [];
  
  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    scales: {
      x: {
        stacked: true,
      },
      y: {
        stacked: true
      }
    },
    plugins: {
      legend: { position: 'top' }
    }
  };

  public barChartType: ChartType = 'bar';

  public barChartData: ChartConfiguration['data'] = {
    labels: [],
    datasets: []
  };

  constructor(
    private authService: AuthService, 
    private friendshipService: FriendshipService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.getStats();
  }

  getStats() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId) return;
      this.friendshipService.getSentFriendRequestStats(userId).subscribe((stats: FriendshipRequestStatsDto[]) => {
        this.sentRequestStats = stats;
        this.processChartData();
        this.cdr.detectChanges();
      });
    });
  }

  processChartData(): void {
    const labels = Array.from(new Set(this.sentRequestStats.map(d => {
      const date = new Date(d.inviteTime);
      return date.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
    })));
  
    const friendshipStatuses = Array.from(new Set(this.sentRequestStats.map(d => d.friendshipStatus)));
  
    const colorMap = {
      'Accepted': 'rgba(75, 192, 192, 0.6)',
      'Rejected': 'rgba(255, 99, 132, 0.6)', 
    };
  
    this.barChartData.labels = labels;
  
    this.barChartData.datasets = friendshipStatuses.map(friendshipStatus => {
      return {
        label: friendshipStatus,
        data: labels.map(label => {
          const entry = this.sentRequestStats.find(d => {
            const date = new Date(d.inviteTime).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
            return date === label && d.friendshipStatus === friendshipStatus;
          });
          return entry ? entry.count : 0;
        }),
        backgroundColor: colorMap[friendshipStatus as keyof typeof colorMap] || 'rgba(150, 150, 150, 0.6)',
      };
    });
  }
  
}
