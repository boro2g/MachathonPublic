<template>
  <div>
    <div class="md-layout">
      <div class="md-layout-item">
        <h1 class="md-display-3">Machathon Moodboard</h1>
      </div>
    </div>
    <div class="md-layout">
      <div
        class="md-layout md-layout-item md-xlarge-size-50 md-medium-size-50 md-small-size-100 md-xsmall-size-100 md-alignment-top-center"
        v-for="teamMember in teamMembers"
        v-bind:key="teamMember.name"
        :style="setBg(teamMember.backgroundColour)"
      >
        <div
          class="md-layout-item md-size-25"
          :style="setBg(teamMember.backgroundColour)"
        >
          <img
            :src="
              teamMember.photo.url +
                '?w=200&h=200&fit=fill&fm=jpg&fl=progressive'
            "
          />
        </div>
        <div class="md-layout-item" :style="setBg(teamMember.backgroundColour)">
          <h2 class="md-display-3">
            {{ teamMember.name }}
            <span v-html="lookupEmoji(teamMember.mood.moodKey)" />
          </h2>

          <p class="md-body-1" style="padding-top:10px;">
            {{ teamMember.outcome }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { createClient } from "~/plugins/contentful.js";
import loadTeamMembersQuery from "~/apollo/queries/loadTeamMembers.gql";

const client = createClient();

function buildTeamMember(newData, teamMember) {
  if (newData.Outcome.length > 0) {
    teamMember.outcome = newData.Outcome;
  }
  teamMember.mood.moodKey = newData.Usermood;
  return teamMember;
}

export default {
  data() {
    return {
      teamMembers: []
    };
  },
  methods: {
    setBg(backgroundColour) {
      return `background: ${backgroundColour};`;
    },
    lookupEmoji(moodKey) {
      if (moodKey == "Happy") {
        return "&#128512;";
      }
      if (moodKey == "Sad") {
        return "&#128543;";
      }
      return "&#128528;";
    }
  },
  sockets: {
    newMessage(data) {
      this.teamMembers = this.teamMembers.map(teamMember =>
        teamMember.name.toLowerCase() === data.Username.toLowerCase()
          ? buildTeamMember(data, teamMember)
          : teamMember
      );
    }
  },

  apollo: {
    teamMemberCollection: {
      variables() {
        //return { name: "Nick" };
      },
      manual: true,
      query: loadTeamMembersQuery,
      result({ data, loading }) {
        if (!loading) {
          this.teamMembers = data.teamMemberCollection.items;
          function lookupOutcome(
            teamMember,
            specificOutcomes,
            defaultOutcomes
          ) {
            let specificOutcome = specificOutcomes.filter(
              a =>
                a.mood.moodKey === teamMember.mood.moodKey &&
                a.teamMember.name === teamMember.name
            );

            if (
              specificOutcome != undefined &&
              specificOutcome.length != 0 &&
              specificOutcome[0].outcome.toLowerCase() != "default"
            ) {
              return specificOutcome[0].outcome;
            }

            let defaultOutcome = defaultOutcomes.filter(
              a => a.mood.moodKey === teamMember.mood.moodKey
            );

            return defaultOutcome[0].outcome;
          }
          this.teamMembers.forEach((name, index) => {
            this.teamMembers[index].outcome = lookupOutcome(
              this.teamMembers[index],
              data.specificOutcomes.items,
              data.defaultOutcomes.items
            );
          });
        }
      }
    }
  },
  asyncData({ env }) {
    return Promise.all([
      client.getEntries({
        content_type: "teamMember",
        order: "-sys.createdAt"
      })
    ])
      .then(([entries]) => {
        return {
          team: entries.items
        };
      })
      .catch(console.error);
  }
};
</script>
<style lang="scss" scoped>
@import "~vue-material/dist/theme/engine";
.md-layout-item {
  padding: 20px;
  // margin:20px;
  transition: 0.3s 0.3s;
  background: #eee;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.1s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>


