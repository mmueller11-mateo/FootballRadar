(function () {
    const raw = JSON.parse(document.getElementById('bracket-data').textContent);
    const S = {};
    for (const [k, v] of Object.entries(raw.rounds)) {
        S[k] = v.map(m => ({ ...m, winner: m.winnerId ?? null }));
    }

    function winnerName(m) {
        if (!m.winner) return null;
        if (m.homeId && m.winner === m.homeId) return m.home;
        if (m.awayId && m.winner === m.awayId) return m.away;
        return null;
    }

    function propagate() {
        const pairs = [['r32_L', 'r16_L'], ['r32_R', 'r16_R'], ['r16_L', 'qf_L'], ['r16_R', 'qf_R']];
        for (const [src, dst] of pairs) {
            for (let i = 0; i < S[dst].length; i++) {
                const srcH = S[src][i * 2];
                const srcA = S[src][i * 2 + 1];
                const wH = srcH.winner ? (srcH.winner === srcH.homeId
                    ? { name: srcH.home, id: srcH.homeId, logo: srcH.homeLogo }
                    : { name: srcH.away, id: srcH.awayId, logo: srcH.awayLogo }) : null;
                const wA = srcA.winner ? (srcA.winner === srcA.homeId
                    ? { name: srcA.home, id: srcA.homeId, logo: srcA.homeLogo }
                    : { name: srcA.away, id: srcA.awayId, logo: srcA.awayLogo }) : null;

                S[dst][i].home = wH?.name ?? '?';
                S[dst][i].homeId = wH?.id ?? null;
                S[dst][i].homeLogo = wH?.logo ?? null;

                S[dst][i].away = wA?.name ?? '?';
                S[dst][i].awayId = wA?.id ?? null;
                S[dst][i].awayLogo = wA?.logo ?? null;

                if (S[dst][i].winner !== S[dst][i].homeId && S[dst][i].winner !== S[dst][i].awayId)
                    S[dst][i].winner = null;
            }
        }

        ['sf_L', 'sf_R'].forEach((sf, si) => {
            const src = si === 0 ? 'qf_L' : 'qf_R';
            const wH = S[src][0].winner ? (S[src][0].winner === S[src][0].homeId
                ? { name: S[src][0].home, id: S[src][0].homeId, logo: S[src][0].homeLogo }
                : { name: S[src][0].away, id: S[src][0].awayId, logo: S[src][0].awayLogo }) : null;
            const wA = S[src][1].winner ? (S[src][1].winner === S[src][1].homeId
                ? { name: S[src][1].home, id: S[src][1].homeId, logo: S[src][1].homeLogo }
                : { name: S[src][1].away, id: S[src][1].awayId, logo: S[src][1].awayLogo }) : null;

            S[sf][0].home = wH?.name ?? '?'; S[sf][0].homeId = wH?.id ?? null; S[sf][0].homeLogo = wH?.logo ?? null;
            S[sf][0].away = wA?.name ?? '?'; S[sf][0].awayId = wA?.id ?? null; S[sf][0].awayLogo = wA?.logo ?? null;
            if (S[sf][0].winner !== S[sf][0].homeId && S[sf][0].winner !== S[sf][0].awayId) S[sf][0].winner = null;
        });

        if (S.final[0]) {
            const wL = S.sf_L[0].winner ? (S.sf_L[0].winner === S.sf_L[0].homeId
                ? { name: S.sf_L[0].home, id: S.sf_L[0].homeId, logo: S.sf_L[0].homeLogo }
                : { name: S.sf_L[0].away, id: S.sf_L[0].awayId, logo: S.sf_L[0].awayLogo }) : null;
            const wR = S.sf_R[0].winner ? (S.sf_R[0].winner === S.sf_R[0].homeId
                ? { name: S.sf_R[0].home, id: S.sf_R[0].homeId, logo: S.sf_R[0].homeLogo }
                : { name: S.sf_R[0].away, id: S.sf_R[0].awayId, logo: S.sf_R[0].awayLogo }) : null;

            S.final[0].home = wL?.name ?? '?'; S.final[0].homeId = wL?.id ?? null; S.final[0].homeLogo = wL?.logo ?? null;
            S.final[0].away = wR?.name ?? '?'; S.final[0].awayId = wR?.id ?? null; S.final[0].awayLogo = wR?.logo ?? null;
            if (S.final[0].winner !== S.final[0].homeId && S.final[0].winner !== S.final[0].awayId) S.final[0].winner = null;
        }

        if (S.third[0]) {
            const lL = S.sf_L[0].winner
                ? (S.sf_L[0].winner === S.sf_L[0].homeId
                    ? { name: S.sf_L[0].away, id: S.sf_L[0].awayId, logo: S.sf_L[0].awayLogo }
                    : { name: S.sf_L[0].home, id: S.sf_L[0].homeId, logo: S.sf_L[0].homeLogo }) : null;
            const lR = S.sf_R[0].winner
                ? (S.sf_R[0].winner === S.sf_R[0].homeId
                    ? { name: S.sf_R[0].away, id: S.sf_R[0].awayId, logo: S.sf_R[0].awayLogo }
                    : { name: S.sf_R[0].home, id: S.sf_R[0].homeId, logo: S.sf_R[0].homeLogo }) : null;

            S.third[0].home = lL?.name ?? '?'; S.third[0].homeId = lL?.id ?? null; S.third[0].homeLogo = lL?.logo ?? null;
            S.third[0].away = lR?.name ?? '?'; S.third[0].awayId = lR?.id ?? null; S.third[0].awayLogo = lR?.logo ?? null;
            if (S.third[0].winner !== S.third[0].homeId && S.third[0].winner !== S.third[0].awayId) S.third[0].winner = null;
        }
    }

    async function saveTip(fixtureId, winnerId) {
        await fetch('/TippMatch/SaveKoTip', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value ?? ''
            },
            body: JSON.stringify({ fixtureId, winnerId })
        });
    }

    function pick(roundKey, idx, isHome) {
        const m = S[roundKey][idx];
        const teamId = isHome ? m.homeId : m.awayId;
        const teamName = isHome ? m.home : m.away;
        if (!teamId || teamName === '?') return;
        m.winner = (m.winner === teamId) ? null : teamId;
        propagate();
        render();
        saveTip(m.fixtureId, m.winner);
    }

    function card(rk, idx, isFinal) {
        const m = S[rk][idx];
        const hW = m.winner && m.winner === m.homeId;
        const aW = m.winner && m.winner === m.awayId;
        const hUnk = !m.homeId || m.home === '?';
        const aUnk = !m.awayId || m.away === '?';
        const cls = isFinal === 'final' ? 'match-card final-card' : isFinal === 'third' ? 'match-card third-card' : 'match-card';
        return `<div class="${cls}">
            <div class="team-row${hW ? ' winner' : ''}" onclick="bracketPick('${rk}',${idx},true)">
                ${m.homeLogo ? `<img src="${m.homeLogo}" style="width:14px;height:14px;object-fit:contain" />` : ''}
                <span class="tname${hUnk ? ' unknown' : ''}">${m.home}</span>
            </div>
            <div class="team-row${aW ? ' winner' : ''}" onclick="bracketPick('${rk}',${idx},false)">
                ${m.awayLogo ? `<img src="${m.awayLogo}" style="width:14px;height:14px;object-fit:contain" />` : ''}
                <span class="tname${aUnk ? ' unknown' : ''}">${m.away}</span>
            </div>
        </div>`;
    }

    function conns(n, rev) {
        let h = '';
        for (let i = 0; i < n; i++) {
            if (i % 2 === 0) h += `<div class="conn-wrap" style="flex:2"><div class="conn-pair${rev ? ' conn-pair-rev' : ''}"><div class="top"></div><div class="bot"></div></div></div>`;
            else h += `<div class="conn-wrap" style="flex:2"><div style="width:14px;height:1px;background:var(--bs-border-color-translucent)"></div></div>`;
        }
        return h;
    }

    function col(label, rk, align) {
        return `<div class="round-col" style="text-align:${align || 'left'}">
            <div class="round-lbl">${label}</div>
            <div class="matches-stack">${S[rk].map((_, i) => `<div class="match-wrap" style="flex:1">${card(rk, i, false)}</div>`).join('')}</div>
        </div>`;
    }

    function render() {
        propagate();
        document.getElementById('ko-bracket').innerHTML = `
        <div style="display:flex;align-items:stretch;flex:1">
            ${col('Rd. 32', 'r32_L')}
            <div class="conn-col">${conns(8, false)}</div>
            ${col('Rd. 16', 'r16_L')}
            <div class="conn-col">${conns(4, false)}</div>
            ${col('Viertelfinale', 'qf_L')}
            <div class="conn-col">${conns(2, false)}</div>
            ${col('Halbfinale', 'sf_L')}
            <div class="conn-col" style="width:14px"><div class="conn-wrap" style="flex:1"><div style="width:14px;height:1px;background:var(--bs-border-color-translucent)"></div></div></div>
        </div>
        <div class="final-col">
            <div class="round-lbl">Finale</div>
            <div style="font-size:18px">🏆</div>
            ${card('final', 0, 'final')}
            <div class="round-lbl" style="margin-top:8px">Platz 3</div>
            ${card('third', 0, 'third')}
        </div>
        <div style="display:flex;align-items:stretch;flex:1;flex-direction:row-reverse">
            ${col('Rd. 32', 'r32_R', 'right')}
            <div class="conn-col">${conns(8, true)}</div>
            ${col('Rd. 16', 'r16_R', 'right')}
            <div class="conn-col">${conns(4, true)}</div>
            ${col('Viertelfinale', 'qf_R', 'right')}
            <div class="conn-col">${conns(2, true)}</div>
            ${col('Halbfinale', 'sf_R', 'right')}
            <div class="conn-col" style="width:14px"><div class="conn-wrap" style="flex:1"><div style="width:14px;height:1px;background:var(--bs-border-color-translucent)"></div></div></div>
        </div>`;
    }

    window.bracketPick = pick;
    render();
})();